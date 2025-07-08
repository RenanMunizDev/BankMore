
using FluentValidation;

namespace ContaCorrenteAPI.Application.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.CpfOuConta).NotEmpty();
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
        }
    }
}
