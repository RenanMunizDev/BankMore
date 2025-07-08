using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ContaCorrenteAPI.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly string _connectionString;

    public ContaCorrenteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null.");
    }

    public async Task CriarAsync(ContaCorrente conta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO ContaCorrente (Id, Agencia, NumeroConta, NomeTitular, Cpf, Ativo, SenhaHash, Saldo)
                            VALUES (@Id, @Agencia, @NumeroConta, @NomeTitular, @Cpf, @Ativo, @SenhaHash, @Saldo)";
        await conn.ExecuteAsync(sql, new
        {
            conta.Id,
            conta.Agencia,
            conta.NumeroConta,
            conta.NomeTitular,
            conta.Cpf,
            conta.Ativo,
            conta.SenhaHash,
            conta.Saldo
        });
    }

    public async Task AtualizarAsync(ContaCorrente conta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"UPDATE ContaCorrente SET SenhaHash = @SenhaHash, NomeTitular = @NomeTitular, Ativo = @Ativo, Saldo = @Saldo
                            WHERE Id = @Id";
        await conn.ExecuteAsync(sql, new
        {
            conta.Id,
            conta.SenhaHash,
            conta.NomeTitular,
            conta.Ativo,
            conta.Saldo
        });
    }

    public async Task<IEnumerable<ContaCorrente>> ListarTodosAsync()
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM ContaCorrente";
        return await conn.QueryAsync<ContaCorrente>(sql);
    }

    public async Task<ContaCorrente> ObterPorIdAsync(string id)
    {
        using var conn = new SqliteConnection(_connectionString);

        var sql = "SELECT * FROM ContaCorrente WHERE Id = @Id COLLATE NOCASE";

        var result = await conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Id = id });

        if (result == null)
        {
            throw new InvalidOperationException($"ContaCorrente with Id '{id}' was not found.");
        }

        return result;
    }
    public Task<ContaCorrente?> ObterPorCpfOuContaAsync(string cpfOuConta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM ContaCorrente WHERE Cpf = @CpfOuConta OR NumeroConta = @CpfOuConta";
        return conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { CpfOuConta = cpfOuConta });
    }

    public Task<ContaCorrente?> ObterPorNumeroAsync(string numeroConta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM ContaCorrente WHERE NumeroConta = @NumeroConta";
        return conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { NumeroConta = numeroConta });
    }
}
