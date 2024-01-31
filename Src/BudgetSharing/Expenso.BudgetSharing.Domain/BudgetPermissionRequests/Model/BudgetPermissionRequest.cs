using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;

public sealed class BudgetPermissionRequest
{
    private readonly DomainEventsSource _domainEventsSource = new();
    private readonly DomainModelState _domainModelState = new();

    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private BudgetPermissionRequest()
    {
    }

    private BudgetPermissionRequest(BudgetPermissionRequestId id, BudgetId budgetId, PersonId participantId,
        PermissionType permissionType, BudgetPermissionRequestStatus status, int expirationDays, IClock clock)
    {
        DateAndTime expirationDate = DateAndTime.Create(clock.UtcNow.AddDays(expirationDays));

        _domainModelState.CheckBusinessRules([
            new UnknownPermissionTypeCannotBeProcessed(permissionType),
            new UnknownBudgetPermissionRequestStatusCannotBeProcessed(status),
            new ExpirationDateMustBeGreaterThanOneDay(expirationDate, clock)
        ]);

        Id = id;
        BudgetId = budgetId;
        ParticipantId = participantId;
        PermissionType = permissionType;
        Status = status;
        ExpirationDate = expirationDate;
        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequested(id));
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public DateAndTime? ExpirationDate { get; private set; }

    public static BudgetPermissionRequest Create(BudgetId budgetId, PersonId personId, PermissionType permissionType,
        int expirationDays, IClock clock)
    {
        return new BudgetPermissionRequest(Guid.NewGuid(), budgetId, personId, permissionType,
            BudgetPermissionRequestStatus.Pending, expirationDays, clock);
    }

    public void Confirm()
    {
        _domainModelState.CheckBusinessRules([new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(Id, Status)]);
        Status = BudgetPermissionRequestStatus.Confirmed;
        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestConfirmed(Id));
    }

    public void Cancel()
    {
        _domainModelState.CheckBusinessRules([new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(Id, Status)]);
        Status = BudgetPermissionRequestStatus.Cancelled;
        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestCancelled(Id));
    }

    public void Expire()
    {
        _domainModelState.CheckBusinessRules([new OnlyPendingBudgetPermissionRequestCanBeMadeExpired(Id, Status)]);
        Status = BudgetPermissionRequestStatus.Expired;
        _domainEventsSource.AddDomainEvent(new BudgetPermissionRequestExpired(Id));
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEventsSource.DomainEvents;
    }
}