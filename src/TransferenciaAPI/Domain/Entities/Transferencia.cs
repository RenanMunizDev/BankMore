namespace TransferenciaAPI.Domain.Entities
{
    public class Transferencia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ContaOrigemNumero { get; set; } = string.Empty;
        public string ContaDestinoNumero { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string IdempotentKey { get; set; } = string.Empty;
    }
}
