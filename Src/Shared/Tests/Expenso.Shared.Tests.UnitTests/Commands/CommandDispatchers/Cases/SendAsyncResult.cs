using System.Text;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatchers.Cases;

internal sealed class SendAsyncResult : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        TestCommand testCommand = new(Guid.NewGuid());

        // Act
        TestCommandResult? commandResult = await TestCandidate.SendAsync<TestCommand, TestCommandResult>(testCommand);

        // Assert
        commandResult?.Should().NotBeNull();
        commandResult?.Message.Should().NotBeNullOrEmpty();

        string message = new StringBuilder()
            .Append("Successfully processed command with id: ")
            .Append(testCommand.Id)
            .ToString();

        commandResult?.Message.Should().Be(message);
    }
}