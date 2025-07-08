using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ContaCorrenteAPI.Application.Handlers
{
    public class InativarContaCommandHandler : IRequestHandler<InativarContaCommand, Unit>
    {
        private readonly IContaCorrenteRepository _repository;

        public InativarContaCommandHandler(IContaCorrenteRepository @object, IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.ObterPorIdAsync(request.ContaId);

            if (conta == null || !conta.Ativo)
                throw new ValidationException("INVALID_ACCOUNT: Conta não encontrada ou desativada");

            if (!BCrypt.Net.BCrypt.Verify(request.Senha, conta.SenhaHash))
                throw new ValidationException("INVALID_PASSWORD: Senha incorreta");

            conta.Ativo = false;
            await _repository.AtualizarAsync(conta);

            return Unit.Value;
        }
    }
}
