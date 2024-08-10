using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

internal sealed class SendAsync : CommandDispatcherTestBase
{
    [Test]
    public void Should_SendCommand()
    {
        // Arrange
        Guid testCommandId = Guid.NewGuid();

        TestCommand testCommand = new(MessageContext: MessageContextFactoryMock.Object.Current(), Id: testCommandId,
            Name: "UsWNuYtfQTtvYR");

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () =>
            TestCandidate.SendAsync(command: testCommand, cancellationToken: It.IsAny<CancellationToken>()));
    }
}