namespace ContaCorrenteAPI.Domain.Entities
{
    public class Movimento
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ContaId { get; set; } = Guid.NewGuid().ToString();
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public decimal Valor { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string IdempotentKey { get; set; } = string.Empty;
    }
}
