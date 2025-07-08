namespace ContaCorrenteAPI.Application.Queries 
{
   public class ConsultarSaldoResult
    {
        public string NumeroConta { get; set; } = string.Empty;
        public string NomeTitular { get; set; } = string.Empty;
        public DateTime DataHoraConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
