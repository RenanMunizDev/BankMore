using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ContaCorrenteAPI.Application.Handlers
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, Unit>
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movimentoRepo;

        public MovimentacaoCommandHandler(IContaCorrenteRepository contaRepo, IMovimentoRepository movimentoRepo)
        {
            _contaRepo = contaRepo;
            _movimentoRepo = movimentoRepo;
        }

        public async Task<Unit> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var conta = request.NumeroConta != null ? await _contaRepo.ObterPorNumeroAsync(request.NumeroConta) : null;

            if (conta == null)
                throw new ValidationException("INVALID_ACCOUNT: Conta não encontrada");

            if (!conta.Ativo)
                throw new ValidationException("INACTIVE_ACCOUNT: Conta inativa");

            if (request.Valor <= 0)
                throw new ValidationException("INVALID_VALUE: Valor deve ser maior que zero");

            var tipo = request.Tipo.ToUpper();
            if (tipo != "C" && tipo != "D")
                throw new ValidationException("INVALID_TYPE: Tipo deve ser 'C' ou 'D'");
            
            if (conta == null)
                throw new ValidationException("INVALID_ACCOUNT: Conta não encontrada");

            if (!conta.Ativo)
                throw new ValidationException("INACTIVE_ACCOUNT: Conta inativa");

            if (tipo == "D" && conta.Id.ToString() != request.ContaIdLogada)
                throw new ValidationException("INVALID_TYPE: Débito só permitido na conta logada");

            var jaExiste = await _movimentoRepo.ExisteMovimentoComIdempotentKeyAsync(conta.Id, request.IdempotentKey);
            if (jaExiste)
                return Unit.Value;

            var movimento = new Movimento
            {
                ContaId = conta.Id,
                Valor = request.Valor,
                Tipo = tipo,
                IdempotentKey = request.IdempotentKey
            };

            await _movimentoRepo.RegistrarAsync(movimento);

            if (tipo == "C")
                conta.Saldo += request.Valor;
            else
                conta.Saldo -= request.Valor;

            await _contaRepo.AtualizarAsync(conta);
            return Unit.Value;
        }
    }
}
