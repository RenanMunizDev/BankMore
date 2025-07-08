using FluentAssertions;
using MediatR;
using Moq;
using TransferenciaAPI.Application.Commands;
using TransferenciaAPI.Application.Handlers;
using TransferenciaAPI.Domain.Entities;
using TransferenciaAPI.Domain.Interfaces;
using Xunit;

namespace TransferenciaAPI.Tests
{
    public class TransferenciaCommandHandlerTests
    {
        [Fact]
        public async Task Transferencia_Valida_DeveRealizarDebitoCreditoEPersistir()
        {
            var mockService = new Mock<IContaCorrenteService>();
            var mockRepo = new Mock<ITransferenciaRepository>();

            mockRepo.Setup(r => r.ExisteComIdempotentKeyAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(false);

            var handler = new TransferenciaCommandHandler(mockService.Object, mockRepo.Object);

            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123",
                ContaDestinoNumero = "456",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            mockService.Verify(s => s.DebitarAsync("123", 100.0m), Times.Once);
            mockService.Verify(s => s.CreditarAsync("456", 100.0m), Times.Once);
            mockRepo.Verify(r => r.RegistrarAsync(It.Is<Transferencia>(
                t => t.ContaOrigemNumero == "123" &&
                     t.ContaDestinoNumero == "456" &&
                     t.Valor == 100.0m &&
                     t.IdempotentKey == "REQ-001")), Times.Once);

            result.Should().Be(Unit.Value);
        }
    }
}
