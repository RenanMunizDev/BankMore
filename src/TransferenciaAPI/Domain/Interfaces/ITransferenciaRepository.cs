using TransferenciaAPI.Domain.Entities;

namespace TransferenciaAPI.Domain.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task RegistrarAsync(Transferencia transferencia);
        Task<bool> ExisteComIdempotentKeyAsync(string contaOrigemNumero, string idempotentKey);
    }
}
