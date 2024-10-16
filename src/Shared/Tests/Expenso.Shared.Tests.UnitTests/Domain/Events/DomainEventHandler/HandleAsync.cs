using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandler;

[TestFixture]
internal sealed class HandleAsync : DomainEventHandlerTestBase
{
    [Test]
    public async Task Should_HandleDomainEvent()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.GeneralInformation, It.IsAny<string>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }
}