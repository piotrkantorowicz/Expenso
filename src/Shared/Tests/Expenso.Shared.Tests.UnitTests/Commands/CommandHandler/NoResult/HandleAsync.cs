using Expenso.Shared.System.Logging.Constants;

using Moq;

using NUnit.Framework;

using Shouldly;

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
        _testCommand.MessageContext.ShouldNotBeNull();

        _testCommand.MessageContext.CorrelationId.ShouldBe(expected: MessageContextFactoryMock.Object.Current()
            .CorrelationId);

        _testCommand.MessageContext.MessageId.ShouldBe(expected: MessageContextFactoryMock.Object.Current().MessageId);

        _testCommand.MessageContext.RequestedBy.ShouldBe(expected: MessageContextFactoryMock.Object.Current()
            .RequestedBy);

        _testCommand.MessageContext.Timestamp.ShouldBe(expected: MessageContextFactoryMock.Object.Current().Timestamp);
        _testCommand.MessageContext.ModuleId.ShouldBe(expected: MessageContextFactoryMock.Object.Current().ModuleId);
    }
}