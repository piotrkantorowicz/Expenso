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
        TestCommandResult? commandResult = await TestCandidate.HandleAsync(_testCommand, It.IsAny<CancellationToken>());

        // Assert
        commandResult?.Should().NotBeNull();
        commandResult?.Message.Should().NotBeNullOrEmpty();
        string message = $"Successfully processed command with id: {_testCommand.Id}";
        commandResult?.Message.Should().Be(message);
    }
}