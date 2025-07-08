
using MediatR;
using System.Text.Json.Serialization;

namespace ContaCorrenteAPI.Application.Commands
{
    public class InativarContaCommand : IRequest<Unit>
    {
        public string Senha { get; set; } = string.Empty;

        [JsonIgnore]
        public string ContaId { get; set; } = string.Empty; 
    }
}
