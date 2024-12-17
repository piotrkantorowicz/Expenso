using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;
using Expenso.Shared.Domain.Types.ValueObjects;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionBlockedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<
    BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionBlockedEventHandler,
    BudgetPermissionBlockedEvent>
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

    protected override BudgetPermissionBlockedEvent CreateEvent()
    {
        return new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, BlockDate: DateAndTime.New(value: _clock.Object.UtcNow), BudgetCode: _budgetCode,
            Permissions:
            [Permission.Create(participantId: _defaultParticipantId, permissionType: PermissionType.SubOwner)]);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionBlockedEventHandler(
                communicationProxy: _communicationProxyMock.Object, notificationSettings: _notificationSettings,
                iamProxyService: _iIamProxyServiceMock.Object);
    }
}