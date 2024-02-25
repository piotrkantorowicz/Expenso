using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

internal sealed class SendAsyncResult : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        TestCommand testCommand = new(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "BzC6M2Qjw7Y2CPC4s");

        // Act
        TestCommandResult? commandResult =
            await TestCandidate.SendAsync<TestCommand, TestCommandResult>(testCommand, It.IsAny<CancellationToken>());

        // Assert
        commandResult?.Should().NotBeNull();
        commandResult?.Message.Should().NotBeNullOrEmpty();
        string message = $"Successfully processed command with id: {testCommand.Id}";
        commandResult?.Message.Should().Be(message);
    }
}