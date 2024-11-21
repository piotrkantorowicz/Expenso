using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionUnblockedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<
    BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionUnblockedEventHandler,
    BudgetPermissionUnblockedEvent>
{
    public override async Task Should_SendNotification_For_Recipients()
    {
        await Should_SendNotification_For_Recipients_Internal(notificationCount: Times.AtLeast(callCount: 3));
    }

    public override async Task Should_Call_GetUserNotificationAvailability_With_MessageContext()
    {
        await Should_Call_GetUserNotificationAvailability_With_MessageContext_Internal(
            notificationCount: Times.AtLeast(callCount: 3));
    }

    protected override BudgetPermissionUnblockedEvent CreateEvent()
    {
        return new BudgetPermissionUnblockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            BudgetCode: _budgetCode,
            OwnerId: _defaultOwnerId,
            Permissions:
            [Permission.Create(participantId: _defaultParticipantId, permissionType: PermissionType.SubOwner)]);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionUnblockedEventHandler(
                communicationProxy: _communicationProxyMock.Object, notificationSettings: _notificationSettings,
                iamProxyService: _iIamProxyServiceMock.Object);
    }
}