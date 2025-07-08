
using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace ContaCorrenteAPI.Application.Handlers
{
    public class CadastrarContaCommandHandler : IRequestHandler<CadastrarContaCommand, string>
    {
        private readonly IContaCorrenteRepository _repository;

        public CadastrarContaCommandHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CadastrarContaCommand request, CancellationToken cancellationToken)
        {
            if (!Regex.IsMatch(request.Cpf, @"^\d{11}$"))
                throw new ValidationException("INVALID_DOCUMENT: CPF inválido.");

            var conta = new ContaCorrente
            {
                Cpf = request.Cpf,
                NomeTitular = request.NomeTitular,
                NumeroConta = Guid.NewGuid().ToString().Substring(0, 8),
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Agencia = request.Agencia,
            };

            await _repository.CriarAsync(conta);
            return conta.NumeroConta;
        }
    }
}
