using FluentValidation;
using TransferenciaAPI.Application.Commands;

namespace TransferenciaAPI.Application.Validators
{
    public class TransferenciaCommandValidator : AbstractValidator<TransferenciaCommand>
    {
        public TransferenciaCommandValidator()
        {
            RuleFor(x => x.ContaDestinoNumero).NotEmpty();
            RuleFor(x => x.Valor).GreaterThan(0);
            RuleFor(x => x.IdempotentKey).NotEmpty();
        }
    }
}
