﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransferenciaAPI.Application.Commands;

namespace TransferenciaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaCommand command)
        {
            var NumeroConta = User.FindFirst("contaOrigemNumero")?.Value;

            if (string.IsNullOrWhiteSpace(NumeroConta))
                return Forbid();

            command.ContaOrigemNumero = NumeroConta;

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
