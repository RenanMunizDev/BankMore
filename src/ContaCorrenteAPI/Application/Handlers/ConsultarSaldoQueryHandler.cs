using ContaCorrenteAPI.Application.Queries;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ContaCorrenteAPI.Application.Handlers 
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResult>
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movimentoRepo;

        public ConsultarSaldoQueryHandler(IContaCorrenteRepository contaRepo, IMovimentoRepository movimentoRepo)
        {
            _contaRepo = contaRepo;
            _movimentoRepo = movimentoRepo;
        }

        public async Task<ConsultarSaldoResult> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepo.ObterPorIdAsync(request.ContaId.ToString());
            if (conta == null)
                throw new ValidationException("INVALID_ACCOUNT: Conta não encontrada");

            if (!conta.Ativo)
                throw new ValidationException("INACTIVE_ACCOUNT: Conta inativa");

            var movimentos = await _movimentoRepo.ObterPorContaIdAsync(conta.Id);
            var saldo = movimentos.Sum(m => m.Tipo == "C" ? m.Valor : -m.Valor);

            return new ConsultarSaldoResult
            {
                NumeroConta = conta.NumeroConta,
                NomeTitular = conta.NomeTitular,
                DataHoraConsulta = DateTime.Now,
                SaldoAtual = saldo
            };
        }
    }
}
