using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using TransferenciaAPI.Application.Commands;
using TransferenciaAPI.Domain.Interfaces;
using TransferenciaAPI.Infrastructure.Repositories;
using SQLitePCL;
using ContaCorrenteAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.Combine("Data", "ContaCorrente.db");
var connectionString = $"Data Source={dbPath}";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
var scriptPath = Path.Combine("..", "ContaCorrenteAPI", "Scripts", "003_create_table_Transferencias.sql");
if (File.Exists(scriptPath))
{
    var scriptSql = File.ReadAllText(scriptPath);

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = scriptSql;
    command.ExecuteNonQuery();
}

builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
builder.Services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
builder.Services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

Batteries.Init();

builder.Services.AddHttpClient<IContaCorrenteService, ContaCorrenteService>((provider, client) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["Services:ContaCorrente"];
    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("A URL do serviço ContaCorrente não foi configurada corretamente.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<TransferenciaCommand>(); 
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TransferenciaAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o esquema Bearer. 
                        Exemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.WebHost.UseUrls("http://*:80");

var configuration = builder.Configuration;
var secret = configuration["JwtSettings:Secret"];
if (string.IsNullOrEmpty(secret))
    throw new InvalidOperationException("JwtSettings:Secret não foi configurado!");

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });



builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
