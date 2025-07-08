
using MediatR;

namespace ContaCorrenteAPI.Application.Commands
{
    public class CadastrarContaCommand : IRequest<string>
    {
        public string Cpf { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string NomeTitular { get; set; } = string.Empty;
        public string Agencia { get; set; } = string.Empty;

    }
}
