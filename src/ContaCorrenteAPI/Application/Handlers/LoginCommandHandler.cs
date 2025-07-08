
using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContaCorrenteAPI.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IContaCorrenteRepository _repository;
        private readonly IConfiguration _config;

        public LoginCommandHandler(IContaCorrenteRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.ObterPorCpfOuContaAsync(request.CpfOuConta);

            if (conta == null || !BCrypt.Net.BCrypt.Verify(request.Senha, conta.SenhaHash))
                throw new ValidationException("USER_UNAUTHORIZED: CPF/Conta ou senha inválidos.");

            var secret = _config["JwtSettings:Secret"] ?? throw new InvalidOperationException("JwtSettings:Secret não configurado.");
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ContaId", conta.Id),
                    new Claim("contaOrigemNumero", conta.NumeroConta),
                    new Claim("Nome", conta.NomeTitular)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
