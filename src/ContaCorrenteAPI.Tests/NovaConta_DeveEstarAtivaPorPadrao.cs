using Xunit;
using FluentAssertions;
using ContaCorrenteAPI.Domain.Entities;

namespace ContaCorrenteAPI.Tests.Domain.Entities
{
    public class ContaCorrenteTests
    {
        [Fact]
        public void NovaConta_DeveEstarAtivaPorPadrao()
        {
            var conta = new ContaCorrente
            {
                NumeroConta = "123456",
                NomeTitular = "Renan",
                Cpf = "11111111111",
                SenhaHash = "senhaHash"
            };

            conta.Ativo.Should().BeTrue();
        }
    }
}
