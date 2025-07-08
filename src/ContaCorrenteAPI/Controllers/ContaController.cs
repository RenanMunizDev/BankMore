using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Application.DTOs;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContaCorrenteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movimentoRepo;

        public ContaController(IMediator mediator, IContaCorrenteRepository contaRepo, IMovimentoRepository movimentoRepo)
        {
            _mediator = mediator;
            _contaRepo = contaRepo;
            _movimentoRepo = movimentoRepo;
        }

        [Authorize]
        [HttpPatch("inativar")]
        public async Task<IActionResult> Inativar([FromBody] InativarContaCommand command)
        {
            try
            {
                var contaIdClaim = User.FindFirst("ContaId")?.Value;

                if (string.IsNullOrEmpty(contaIdClaim) || !Guid.TryParse(contaIdClaim, out var contaId))
                    return Forbid();

                command.ContaId = contaIdClaim;

                await _mediator.Send(command);

                return NoContent(); 
            }
            catch (ValidationException ex)
            {
                var partes = ex.Message.Split(':');
                var tipo = partes.Length > 1 ? partes[0].Trim() : "ERRO_DESCONHECIDO";
                var mensagem = partes.Length > 1 ? partes[1].Trim() : ex.Message;

                return BadRequest(new { tipo, mensagem }); 
            }
        }

        [Authorize]
        [HttpPatch("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoCommand command)
        {
            var contaIdClaim = User.FindFirst("ContaId")?.Value;

            if (string.IsNullOrEmpty(contaIdClaim) || !Guid.TryParse(contaIdClaim, out var contaId))
                return Forbid();

            command.ContaIdLogada = contaIdClaim;

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, tipo = ex.Message.Split(':')[0] });
            }
        }

        [HttpPost("debito")]
        public async Task<IActionResult> Debitar([FromBody] MovimentacaoDto dto)
        {
            var conta = await _contaRepo.ObterPorNumeroAsync(dto.NumeroConta);

            if (conta == null)
                return NotFound(new { tipo = "INVALID_ACCOUNT", mensagem = "Conta de origem não encontrada." });

            if (!conta.Ativo)
                return BadRequest(new { tipo = "INACTIVE_ACCOUNT", mensagem = "Conta está inativa." });

            if (dto.Valor <= 0)
                return BadRequest(new { tipo = "INVALID_VALUE", mensagem = "O valor deve ser positivo." });

            if (conta.Saldo < dto.Valor)
                return BadRequest(new { tipo = "INVALID_VALUE", mensagem = "Saldo insuficiente." });

            conta.Saldo -= dto.Valor;
            await _contaRepo.AtualizarAsync(conta);

            return NoContent();
        }

        [HttpPost("credito")]
        public async Task<IActionResult> Creditar([FromBody] MovimentacaoDto dto)
        {
            var conta = await _contaRepo.ObterPorNumeroAsync(dto.NumeroConta);

            if (conta == null)
                return NotFound(new { tipo = "INVALID_ACCOUNT", mensagem = "Conta de destino não encontrada." });

            if (!conta.Ativo)
                return BadRequest(new { tipo = "INACTIVE_ACCOUNT", mensagem = "Conta está inativa." });

            if (dto.Valor <= 0)
                return BadRequest(new { tipo = "INVALID_VALUE", mensagem = "O valor deve ser positivo." });

            conta.Saldo += dto.Valor;
            await _contaRepo.AtualizarAsync(conta);

            return NoContent();
        }

        [HttpGet("saldo")]
        [Authorize]
        public async Task<IActionResult> ObterSaldo()
        {
            var contaIdClaim = User.FindFirst("ContaId")?.Value;

            if (string.IsNullOrEmpty(contaIdClaim) || !Guid.TryParse(contaIdClaim, out var contaId))
                return Forbid();

            var conta = await _contaRepo.ObterPorIdAsync(contaId.ToString());
            if (conta == null)
                throw new ValidationException("INVALID_ACCOUNT: Conta não encontrada");

            if (!conta.Ativo)
                throw new ValidationException("INACTIVE_ACCOUNT: Conta está inativa");

            var movimentos = await _movimentoRepo.ObterPorContaIdAsync(contaId.ToString());
            var saldo = movimentos.Sum(m => m.Tipo == "C" ? m.Valor : -m.Valor);

            var response = new
            {
                conta.NumeroConta,
                conta.NomeTitular,
                DataConsulta = DateTime.UtcNow,
                Saldo = saldo
            };

            return Ok(response);
        }
    }
}
