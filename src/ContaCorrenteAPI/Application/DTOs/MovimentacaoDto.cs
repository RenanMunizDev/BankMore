using System.Text.Json.Serialization;

namespace ContaCorrenteAPI.Application.DTOs
{
    public class MovimentacaoDto
    {
        [JsonPropertyName("numeroConta")]
        public string NumeroConta { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}