using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

internal sealed class HandleAsync : CommandHandlerNoResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        _loggerMock.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));

        // Act
        await TestCandidate.HandleAsync(_testCommand);

        // Assert
        _loggerMock.Verify(
            x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public void Should_TrackMessageContext()
    {
        // Arrange
        // Act
        // Assert
        _testCommand.MessageContext.Should().NotBeNull();
        _testCommand.MessageContext.CorrelationId.Should().Be(_messageContext.CorrelationId);
        _testCommand.MessageContext.MessageId.Should().Be(_messageContext.MessageId);
        _testCommand.MessageContext.RequestedBy.Should().Be(_messageContext.RequestedBy);
        _testCommand.MessageContext.Timestamp.Should().Be(_messageContext.Timestamp);
    }
}