using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionBlockedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<
    BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionBlockedEventHandler,
    BudgetPermissionBlockedEvent>
{
    protected override BudgetPermissionBlockedEvent CreateEvent()
    {
        return new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, BlockDate: DateAndTime.New(value: _clock.Object.UtcNow),
            Permissions:
            [Permission.Create(participantId: _defaultParticipantId, permissionType: PermissionType.SubOwner)]);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionBlockedEventHandler(
                communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}