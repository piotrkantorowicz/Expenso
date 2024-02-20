using System.Text;

using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

internal sealed class SendAsyncResult : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        TestCommand testCommand = new(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "BzC6M2Qjw7Y2CPC4s");

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