using Expenso.Shared.System.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.NoResult;

[TestFixture]
internal sealed class HandleAsync : CommandHandlerNoResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.GeneralInformation,
                "Successfully processed command with ID {CommandId}", _testCommand.MessageContext, _testCommand.Id),
            times: Times.Once);
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

        _testCommand.MessageContext.ModuleId.Should().Be(expected: MessageContextFactoryMock.Object.Current().ModuleId);
    }
}