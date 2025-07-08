
using MediatR;

namespace ContaCorrenteAPI.Application.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string CpfOuConta { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
