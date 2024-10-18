using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.EventHandlers.
    BudgetPermissionGrantedEventHandler;

[TestFixture]
internal sealed class HandleAsync : HandleAsyncBase<
    BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionGrantedEventHandler,
    BudgetPermissionGrantedEvent>
{
    protected override BudgetPermissionGrantedEvent CreateEvent()
    {
        return new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, ParticipantId: _defaultParticipantId, PermissionType: PermissionType.SubOwner);
    }

    protected override void InitTestCandidate()
    {
        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal.BudgetPermissionGrantedEventHandler(
                communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}