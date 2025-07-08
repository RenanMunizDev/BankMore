using MediatR;

namespace ContaCorrenteAPI.Application.Queries 
{
    public class ConsultarSaldoQuery : IRequest<ConsultarSaldoResult>
    {
        public Guid ContaId { get; set; }

        public ConsultarSaldoQuery(Guid contaId)
        {
            ContaId = contaId;
        }
    }
}
