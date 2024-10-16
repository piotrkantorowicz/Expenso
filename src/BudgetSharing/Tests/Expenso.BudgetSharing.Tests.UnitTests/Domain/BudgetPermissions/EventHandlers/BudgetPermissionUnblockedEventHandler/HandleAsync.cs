using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionUnblockedEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionUnblockedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<TestCandidate, BudgetPermissionUnblockedEvent>
{
    protected override BudgetPermissionUnblockedEvent CreateEvent()
    {
        return new BudgetPermissionUnblockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId,
            Permissions:
            [Permission.Create(participantId: _defaultParticipantId, permissionType: PermissionType.SubOwner)]);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate = new TestCandidate(communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}