namespace TransferenciaAPI.Domain.Interfaces
{
    public interface IContaCorrenteService
    {
        Task<bool> ContaExisteAsync(Guid contaId);
        Task<bool> ContaEstaAtivaAsync(Guid contaId);
        Task<decimal> ObterSaldoAsync(Guid contaId);
        Task DebitarAsync(string contaOrigemNumero, decimal valor);
        Task CreditarAsync(string contaDestinoNumero, decimal valor);
    }
}
