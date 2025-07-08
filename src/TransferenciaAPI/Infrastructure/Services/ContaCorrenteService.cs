using System.Net.Http.Headers;
using TransferenciaAPI.Domain.Interfaces;

public class ContaCorrenteService : IContaCorrenteService
{
    private readonly HttpClient _httpClient;

    public ContaCorrenteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ContaExisteAsync(Guid contaId)
    {
        var response = await _httpClient.GetAsync($"/api/ContaCorrente/{contaId}/existe");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ContaEstaAtivaAsync(Guid contaId)
    {
        var response = await _httpClient.GetAsync($"/api/ContaCorrente/{contaId}/ativa");
        if (!response.IsSuccessStatusCode)
            return false;

        var ativo = await response.Content.ReadFromJsonAsync<bool>();
        return ativo;
    }

    public async Task<decimal> ObterSaldoAsync(Guid contaId)
    {
        var response = await _httpClient.GetAsync($"/api/ContaCorrente/{contaId}/saldo");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao obter saldo da conta corrente.");

        var saldo = await response.Content.ReadFromJsonAsync<decimal>();
        return saldo;
    }

    public async Task DebitarAsync(string contaOrigemNumero, decimal valor)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Conta/debito")
        {
            Content = JsonContent.Create(new MovimentacaoDto
            {
                NumeroConta = contaOrigemNumero,
                Valor = valor
            })
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao debitar valor da conta origem. Status: {response.StatusCode}. Conteúdo: {content}");
        }
    }
    public async Task CreditarAsync(string contaDestinoNumero, decimal valor)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/Conta/credito")
        {
            Content = JsonContent.Create(new MovimentacaoDto
            {
                NumeroConta = contaDestinoNumero,
                Valor = valor
            })
        };
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao creditar valor na conta destino. Status: {response.StatusCode}. Conteúdo: {content}");
        }
    }
}
