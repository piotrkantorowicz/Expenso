using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

[TestFixture]
internal sealed class HandleAsync : BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase
{
    [Test]
    public void Should_NotThrow()
    {
        // Arrange
        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () => TestCandidate.HandleAsync(
            @event: new BudgetPermissionRequestExpiredIntegrationEvent(
                MessageContext: MessageContextFactoryMock.Object.Current(),
                Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid())),
            cancellationToken: default));
    }
}