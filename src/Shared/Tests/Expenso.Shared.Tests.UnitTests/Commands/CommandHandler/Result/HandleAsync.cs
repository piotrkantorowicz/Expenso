using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandler.Result;

[TestFixture]
internal sealed class HandleAsync : CommandHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        TestCommandResult commandResult =
            await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        commandResult.ShouldNotBeNull();
        commandResult.Message.ShouldNotBeEmpty();
        string message = $"Successfully processed command with ID {_testCommand.Id}";
        commandResult.Message.ShouldBe(expected: message);
    }
}