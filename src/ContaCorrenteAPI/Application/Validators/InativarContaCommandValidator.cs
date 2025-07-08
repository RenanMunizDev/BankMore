
using FluentValidation;

namespace ContaCorrenteAPI.Application.Commands
{
    public class InativarContaCommandValidator : AbstractValidator<InativarContaCommand>
    {
        public InativarContaCommandValidator()
        {
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
        }
    }
}
