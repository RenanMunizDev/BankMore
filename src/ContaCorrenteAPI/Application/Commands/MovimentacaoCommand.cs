using MediatR;
using System.Text.Json.Serialization;

namespace ContaCorrenteAPI.Application.Commands
{
    public class MovimentacaoCommand : IRequest<Unit>
    {
        public string IdempotentKey { get; set; } = Guid.NewGuid().ToString();
        public string? NumeroConta { get; set; } 
        public decimal Valor { get; set; }
        public string Tipo { get; set; } = string.Empty;
        [JsonIgnore]
        public string ContaIdLogada { get; set; } = string.Empty; 
    }
}
