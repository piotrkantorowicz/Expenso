using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.BudgetSharing.Domain.Shared.Base;
using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests;

public sealed class BudgetPermissionRequest : IAggregateRoot
{
    private readonly DomainEventsSource _domainEventsSource;
    private readonly IMessageContextFactory _messageContextFactory;

    // ReSharper disable once UnusedMember.Local
    // Required for EF Core 
    private BudgetPermissionRequest()
    {
        Id = default!;
        BudgetId = default!;
        ParticipantId = default!;
        OwnerId = default!;
        PermissionType = default!;
        StatusTracker = default!;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
    }

    private BudgetPermissionRequest(BudgetPermissionRequestId id, BudgetId budgetId, PersonId participantId,
        PersonId ownerId, PermissionType permissionType, BudgetPermissionRequestStatus status, int expirationDays,
        IClock clock)
    {
        DateAndTime expirationDate = DateAndTime.New(value: clock.UtcNow.AddDays(days: expirationDays));

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new UnknownPermissionTypeCannotBeProcessed(permissionType: permissionType)),
            new BusinesRuleCheck(
                BusinessRule: new ExpirationDateMustBeGreaterThanOneDay(expirationDate: expirationDate, clock: clock))
        ]);

        Id = id;
        BudgetId = budgetId;
        ParticipantId = participantId;
        OwnerId = ownerId;
        PermissionType = permissionType;

        StatusTracker =
            BudgetPermissionRequestStatusTracker.Start(clock: clock, expirationDate: expirationDate, status: status);

        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: participantId,
            PermissionType: permissionType, SubmissionDate: StatusTracker.SubmissionDate));
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId OwnerId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatusTracker StatusTracker { get; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.DomainEvents;
    }

    public static BudgetPermissionRequest Create(BudgetId budgetId, PersonId ownerId, PersonId personId,
        PermissionType permissionType, int expirationDays, IClock clock)
    {
        return new BudgetPermissionRequest(id: BudgetPermissionRequestId.New(value: Guid.NewGuid()), budgetId: budgetId,
            ownerId: ownerId, participantId: personId, permissionType: permissionType,
            status: BudgetPermissionRequestStatus.Pending, expirationDays: expirationDays, clock: clock);
    }

    public void Confirm(IClock clock)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(budgetPermissionRequestId: Id,
                    status: StatusTracker.Status))
        ]);

        StatusTracker.Confirm(clock: clock);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestConfirmedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            PermissionType: PermissionType));
    }

    public void Cancel(IClock clock)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(budgetPermissionRequestId: Id,
                    status: StatusTracker.Status))
        ]);

        StatusTracker.Cancel(clock: clock);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestCancelledEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            PermissionType: PermissionType));
    }

    public void Expire()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeExpired(budgetPermissionRequestId: Id,
                    status: StatusTracker.Status))
        ]);

        StatusTracker.Expire();

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestExpiredEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            PermissionType: PermissionType, ExpirationDate: StatusTracker.ExpirationDate));
    }
}