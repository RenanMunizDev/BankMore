using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrenteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IContaCorrenteRepository _contaRepository;

        public AuthController(IMediator mediator, IContaCorrenteRepository contaRepository)
        {
            _mediator = mediator;
            _contaRepository = contaRepository;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarContaCommand command)
        {
            try
            {
                var numeroConta = await _mediator.Send(command);
                return Ok(new { NumeroConta = numeroConta });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Tipo = "INVALID_DOCUMENT" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                return Ok(new { Token = token });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return Unauthorized(new { Message = ex.Message, Tipo = "USER_UNAUTHORIZED" });
            }
        }

        [HttpGet("todos")]
        public async Task<IActionResult> ListarTodos()
        {
            var contas = await _contaRepository.ListarTodosAsync();
            return Ok(contas);
        }
    }
}
