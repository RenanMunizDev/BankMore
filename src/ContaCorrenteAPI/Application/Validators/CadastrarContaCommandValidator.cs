
using FluentValidation;

namespace ContaCorrenteAPI.Application.Commands
{
    public class CadastrarContaCommandValidator : AbstractValidator<CadastrarContaCommand>
    {
        public CadastrarContaCommandValidator()
        {
            RuleFor(x => x.Cpf).NotEmpty().Length(11);
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
            RuleFor(x => x.NomeTitular).NotEmpty();
        }
    }
}
