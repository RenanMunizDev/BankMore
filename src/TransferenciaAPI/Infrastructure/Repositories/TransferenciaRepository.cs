using Dapper;
using System.Data;
using TransferenciaAPI.Domain.Entities;
using TransferenciaAPI.Domain.Interfaces;

namespace TransferenciaAPI.Infrastructure.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly IDbConnection _connection;

        public TransferenciaRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task RegistrarAsync(Transferencia transferencia)
        {
            var sql = @"INSERT INTO Transferencias (Id, ContaOrigemNumero, ContaDestinoNumero, Valor, Data, IdempotentKey)
                    VALUES (@Id, @ContaOrigemNumero, @ContaDestinoNumero, @Valor, @Data, @IdempotentKey)";
            await _connection.ExecuteAsync(sql, transferencia);
        }

        public async Task<bool> ExisteComIdempotentKeyAsync(string ContaOrigemNumero, string idempotentKey)
        {
            var sql = @"SELECT COUNT(1) FROM Transferencias 
                    WHERE ContaOrigemNumero = @ContaOrigemNumero AND IdempotentKey = @idempotentKey";
            var count = await _connection.ExecuteScalarAsync<int>(sql, new { ContaOrigemNumero, idempotentKey });
            return count > 0;
        }
    }
}
