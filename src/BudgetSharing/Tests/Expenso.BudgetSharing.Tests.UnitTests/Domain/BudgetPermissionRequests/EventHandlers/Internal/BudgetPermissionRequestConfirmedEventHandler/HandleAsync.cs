using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.Internal.
    BudgetPermissionRequestConfirmedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<
    BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal.BudgetPermissionRequestConfirmedEventHandler,
    BudgetPermissionRequestConfirmedEvent>
{
    protected override BudgetPermissionRequestConfirmedEvent CreateEvent()
    {
        return new BudgetPermissionRequestConfirmedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, ParticipantId: _defaultParticipantId, PermissionType: PermissionType.SubOwner);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal.BudgetPermissionRequestConfirmedEventHandler(communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}