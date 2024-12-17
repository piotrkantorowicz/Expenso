using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

[TestFixture]
internal sealed class HandleAsync : BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase
{
    [Test]
    public async Task Should_NotThrow()
    {
        // Arrange
        // Act
        Func<Task> action = () => TestCandidate.HandleAsync(
            @event: new BudgetPermissionRequestExpiredIntegrationEvent(
                MessageContext: MessageContextFactoryMock.Object.Current(),
                Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid())),
            cancellationToken: default);

        // Assert
        await action.Should().NotThrowAsync();
    }
}