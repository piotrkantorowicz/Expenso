using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

internal sealed class HandleAsync : BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase
{
    [Test]
    public void Should_NotThrow()
    {
        // Arrange
        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () =>
            TestCandidate.HandleAsync(
                @event: new BudgetPermissionRequestExpiredIntegrationEvent(
                    MessageContext: MessageContextFactoryMock.Object.Current(),
                    BudgetPermissionRequestId: Guid.NewGuid()), cancellationToken: default));
    }
}