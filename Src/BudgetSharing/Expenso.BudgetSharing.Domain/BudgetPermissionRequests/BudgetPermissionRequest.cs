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
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests;

public sealed class BudgetPermissionRequest : IAggregateRoot
{
    private readonly DomainEventsSource _domainEventsSource;
    private readonly IMessageContextFactory _messageContextFactory;

    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private BudgetPermissionRequest()
    {
        Id = default!;
        BudgetId = default!;
        ParticipantId = default!;
        PermissionType = default!;
        Status = default!;
        ExpirationDate = default!;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
    }

    private BudgetPermissionRequest(BudgetPermissionRequestId id, BudgetId budgetId, PersonId participantId,
        PermissionType permissionType, BudgetPermissionRequestStatus status, int expirationDays, IClock clock)
    {
        DateAndTime expirationDate = DateAndTime.New(clock.UtcNow.AddDays(expirationDays));

        DomainModelState.CheckBusinessRules([
            (new UnknownPermissionTypeCannotBeProcessed(permissionType), false),
            (new UnknownBudgetPermissionRequestStatusCannotBeProcessed(status), false),
            (new ExpirationDateMustBeGreaterThanOneDay(expirationDate, clock), false)
        ]);

        Id = id;
        BudgetId = budgetId;
        ParticipantId = participantId;
        PermissionType = permissionType;
        Status = status;
        ExpirationDate = expirationDate;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();

        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestedEvent(_messageContextFactory.Current(),
            budgetId, participantId, permissionType));
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public DateAndTime? ExpirationDate { get; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.DomainEvents;
    }

    public static BudgetPermissionRequest Create(BudgetId budgetId, PersonId personId, PermissionType permissionType,
        int expirationDays, IClock clock)
    {
        return new BudgetPermissionRequest(BudgetPermissionRequestId.New(Guid.NewGuid()), budgetId, personId,
            permissionType, BudgetPermissionRequestStatus.Pending, expirationDays, clock);
    }

    public void Confirm()
    {
        DomainModelState.CheckBusinessRules([
            (new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(Id, Status), false)
        ]);

        Status = BudgetPermissionRequestStatus.Confirmed;

        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestConfirmedEvent(_messageContextFactory.Current(),
            BudgetId, ParticipantId, PermissionType));
    }

    public void Cancel()
    {
        DomainModelState.CheckBusinessRules([
            (new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(Id, Status), false)
        ]);

        Status = BudgetPermissionRequestStatus.Cancelled;

        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestCancelledEvent(_messageContextFactory.Current(),
            BudgetId, ParticipantId, PermissionType));
    }

    public void Expire()
    {
        DomainModelState.CheckBusinessRules([
            (new OnlyPendingBudgetPermissionRequestCanBeMadeExpired(Id, Status), false)
        ]);

        Status = BudgetPermissionRequestStatus.Expired;

        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestExpiredEvent(_messageContextFactory.Current(),
            BudgetId, ParticipantId, PermissionType, ExpirationDate));
    }
}