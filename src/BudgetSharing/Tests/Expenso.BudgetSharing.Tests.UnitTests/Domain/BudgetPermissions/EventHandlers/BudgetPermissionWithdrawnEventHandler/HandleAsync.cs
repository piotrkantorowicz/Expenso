using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionWithdrawnEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionWithdrawnEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<TestCandidate, BudgetPermissionWithdrawnEvent>
{
    protected override BudgetPermissionWithdrawnEvent CreateEvent()
    {
        return new BudgetPermissionWithdrawnEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, ParticipantId: _defaultParticipantId, PermissionType: PermissionType.SubOwner);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate = new TestCandidate(communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}