using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

[TestFixture]
internal sealed class SendAsync : CommandDispatcherTestBase
{
    [Test]
    public async Task Should_SendCommand()
    {
        // Arrange
        Guid testCommandId = Guid.CreateVersion7();

        TestCommand testCommand = new(MessageContext: MessageContextFactoryMock.Object.Current(), Id: testCommandId,
            Payload: "UsWNuYtfQTtvYR");

        // Act
        Func<Task> action = async () =>
            await TestCandidate.SendAsync(command: testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action.ShouldNotThrowAsync();
    }
}