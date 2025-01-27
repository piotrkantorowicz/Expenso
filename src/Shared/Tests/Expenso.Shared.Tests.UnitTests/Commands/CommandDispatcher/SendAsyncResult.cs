using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

[TestFixture]
internal sealed class SendAsyncResult : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        TestCommand testCommand = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: Guid.CreateVersion7(),
            Payload: "BzC6M2Qjw7Y2CPC4s");

        // Act
        TestCommandResult? commandResult =
            await TestCandidate.SendAsync<TestCommand, TestCommandResult>(command: testCommand,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        commandResult?.ShouldNotBeNull();
        commandResult?.Message.ShouldNotBeEmpty();
        commandResult?.Message.ShouldBe(expected: $"Successfully processed command with ID {testCommand.Id}");
    }
}