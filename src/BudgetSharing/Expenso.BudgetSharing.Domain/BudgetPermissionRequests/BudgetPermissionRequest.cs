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
        Status = default!;
        ExpirationDate = default!;
        SubmissionDate = default!;
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
                BusinessRule: new UnknownBudgetPermissionRequestStatusCannotBeProcessed(status: status)),
            new BusinesRuleCheck(
                BusinessRule: new ExpirationDateMustBeGreaterThanOneDay(expirationDate: expirationDate, clock: clock))
        ]);

        Id = id;
        BudgetId = budgetId;
        ParticipantId = participantId;
        OwnerId = ownerId;
        PermissionType = permissionType;
        Status = status;
        ExpirationDate = expirationDate;
        SubmissionDate = DateAndTime.New(value: clock.UtcNow);
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: participantId,
            PermissionType: permissionType, SubmissionDate: SubmissionDate));
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId OwnerId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public DateAndTime? ExpirationDate { get; }

    public DateAndTime SubmissionDate { get; }

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

    public void Confirm()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(budgetPermissionRequestId: Id,
                    status: Status))
        ]);

        Status = BudgetPermissionRequestStatus.Confirmed;

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestConfirmedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            PermissionType: PermissionType));
    }

    public void Cancel()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(budgetPermissionRequestId: Id,
                    status: Status))
        ]);

        Status = BudgetPermissionRequestStatus.Cancelled;

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
                    status: Status))
        ]);

        Status = BudgetPermissionRequestStatus.Expired;

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestExpiredEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            PermissionType: PermissionType, ExpirationDate: ExpirationDate));
    }
}