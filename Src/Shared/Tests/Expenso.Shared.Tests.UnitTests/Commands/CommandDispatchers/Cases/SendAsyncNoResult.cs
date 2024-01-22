using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatchers.Cases;

internal sealed class SendAsync : CommandDispatcherTestBase
{
    [Test]
    public void Should_SendCommand()
    {
        // Arrange
        Guid testCommandId = Guid.NewGuid();
        TestCommand testCommand = new(testCommandId, "UsWNuYtfQTtvYR");

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => TestCandidate.SendAsync(testCommand));
    }
}