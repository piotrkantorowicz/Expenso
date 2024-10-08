using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

internal sealed class HandleAsync : CommandHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        TestCommandResult? commandResult =
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        commandResult?.Should().NotBeNull();
        commandResult?.Message.Should().NotBeNullOrEmpty();
        string message = $"Successfully processed command with ID {_testCommand.Id}";
        commandResult?.Message.Should().Be(expected: message);
    }
}