using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

internal sealed class HandleAsync : CommandHandlerNoResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        _loggerMock.Setup(expression: x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));

        // Act
        await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.VerifyLog(logLevel: LogLevel.Information);
    }

    [Test]
    public void Should_TrackMessageContext()
    {
        // Arrange
        // Act
        // Assert
        _testCommand.MessageContext.Should().NotBeNull();

        _testCommand
            .MessageContext.CorrelationId.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().CorrelationId);

        _testCommand
            .MessageContext.MessageId.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().MessageId);

        _testCommand
            .MessageContext.RequestedBy.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().RequestedBy);

        _testCommand
            .MessageContext.Timestamp.Should()
            .Be(expected: MessageContextFactoryMock.Object.Current().Timestamp);
    }
}