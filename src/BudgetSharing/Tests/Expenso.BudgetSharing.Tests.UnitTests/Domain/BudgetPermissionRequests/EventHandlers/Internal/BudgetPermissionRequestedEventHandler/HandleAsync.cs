using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;
using Expenso.Shared.Domain.Types.ValueObjects;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal.BudgetPermissionRequestedEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.Internal.
    BudgetPermissionRequestedEventHandler;

internal sealed class HandleAsync : HandleAsyncBase<TestCandidate, BudgetPermissionRequestedEvent>
{
    protected override BudgetPermissionRequestedEvent CreateEvent()
    {
        return new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, ParticipantId: _defaultParticipantId, PermissionType: PermissionType.SubOwner,
            SubmissionDate: DateAndTime.New(value: _clock.Object.UtcNow));
    }

    protected override void InitTestCandidate()
    {
        TestCandidate = new TestCandidate(communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}