using MediatR;

namespace TransferenciaAPI.Application.Commands
{
    public class TransferenciaCommand : IRequest<Unit>
    {
        public string ContaOrigemNumero { get; set; } = string.Empty;
        public string ContaDestinoNumero { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string IdempotentKey { get; set; } = string.Empty;
    }
}
