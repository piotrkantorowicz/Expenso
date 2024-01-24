using System.Text;

using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

internal sealed class HandleAsync : CommandHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        TestCommandResult? commandResult = await TestCandidate.HandleAsync(_testCommand);

        // Assert
        commandResult?.Should().NotBeNull();
        commandResult?.Message.Should().NotBeNullOrEmpty();

        string message = new StringBuilder()
            .Append("Successfully processed command with id: ")
            .Append(_testCommand.Id)
            .ToString();

        commandResult?.Message.Should().Be(message);
    }
}