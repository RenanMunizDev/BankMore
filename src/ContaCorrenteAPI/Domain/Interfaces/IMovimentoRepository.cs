using ContaCorrenteAPI.Domain.Entities;

namespace ContaCorrenteAPI.Domain.Interfaces
{
    public interface IMovimentoRepository
    {
        Task RegistrarAsync(Movimento movimento);
        Task<IEnumerable<Movimento>> ObterPorContaIdAsync(string contaId);
        Task<bool> ExisteMovimentoComIdempotentKeyAsync(string contaId, string idempotentKey);
    }
}
