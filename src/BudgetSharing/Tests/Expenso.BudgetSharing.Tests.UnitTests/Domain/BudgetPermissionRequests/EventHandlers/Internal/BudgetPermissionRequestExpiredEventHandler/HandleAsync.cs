using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;
using Expenso.Shared.Domain.Types.ValueObjects;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal.
    BudgetPermissionRequestExpiredEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.Internal.
    BudgetPermissionRequestExpiredEventHandler;

internal sealed class HandleAsync : HandleAsyncBase<TestCandidate, BudgetPermissionRequestExpiredEvent>
{
    protected override BudgetPermissionRequestExpiredEvent CreateEvent()
    {
        return new BudgetPermissionRequestExpiredEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            OwnerId: _defaultOwnerId, ParticipantId: _defaultParticipantId, PermissionType: PermissionType.SubOwner,
            ExpirationDate: DateAndTime.New(value: _clock.Object.UtcNow.AddDays(days: 2)));
    }

    protected override void InitTestCandidate()
    {
        TestCandidate = new TestCandidate(communicationProxy: _communicationProxyMock.Object,
            notificationSettings: _notificationSettings, iamProxyService: _iIamProxyServiceMock.Object);
    }
}