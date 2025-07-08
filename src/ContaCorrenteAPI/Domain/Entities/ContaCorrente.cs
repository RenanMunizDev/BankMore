
namespace ContaCorrenteAPI.Domain.Entities
{
    public class ContaCorrente
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Agencia { get; set; } = string.Empty;
        public string NumeroConta { get; set; } = string.Empty;
        public string NomeTitular { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public decimal Saldo { get; set; }
    }
}
