using System.Text;

using Expenso.Shared.Tests.UnitTests.Commands.CommandHandlers.Result;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatchers.Cases;

internal sealed class SendAsyncResult : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        TestCommand testCommand = new(Guid.NewGuid());

        // Act
        CommandResult? commandResult = await TestCandidate.SendAsync<TestCommand, CommandResult>(testCommand);

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