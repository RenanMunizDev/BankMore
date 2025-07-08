
using ContaCorrenteAPI.Domain.Entities;

namespace ContaCorrenteAPI.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task CriarAsync(ContaCorrente conta);
        Task<ContaCorrente?> ObterPorCpfOuContaAsync(string cpfOuConta);
        Task<ContaCorrente?> ObterPorNumeroAsync(string numeroConta);
        Task<ContaCorrente> ObterPorIdAsync(string id);
        Task AtualizarAsync(ContaCorrente conta);
        Task<IEnumerable<ContaCorrente>> ListarTodosAsync();
    }
}
