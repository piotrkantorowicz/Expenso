using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.BudgetSharing.Domain.Shared.Base;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.ValueObjects;
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
        BudgetCode = default!;
        ParticipantId = default!;
        OwnerId = default!;
        PermissionType = default!;
        StatusTracker = default!;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
    }

    private BudgetPermissionRequest(BudgetPermissionRequestId id, BudgetId budgetId, BudgetCode budgetCode,
        PersonId participantId, PersonId ownerId, PermissionType permissionType, BudgetPermissionRequestStatus status,
        DateAndTime expirationDate, DateAndTime submissionDate)
    {
        Id = id;
        BudgetId = budgetId;
        BudgetCode = budgetCode;
        ParticipantId = participantId;
        OwnerId = ownerId;
        PermissionType = permissionType;

        StatusTracker = BudgetPermissionRequestStatusTracker.Start(budgetPermissionRequestId: Id,
            submissionDate: submissionDate, expirationDate: expirationDate, status: status);

        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: participantId,
            BudgetCode: BudgetCode, PermissionType: permissionType, SubmissionDate: StatusTracker.SubmissionDate));
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public BudgetCode BudgetCode { get; }

    public PersonId OwnerId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatusTracker StatusTracker { get; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.GetDomainEvents();
    }

    internal static BudgetPermissionRequest Create(BudgetPermissionRequestId? budgetPermissionRequestId,
        BudgetId budgetId, BudgetCode budgetCode, PersonId ownerId, PersonId personId, PermissionType permissionType,
        DateAndTime expirationDate, DateAndTime submissionDate)
    {
        return new BudgetPermissionRequest(
            id: budgetPermissionRequestId ?? BudgetPermissionRequestId.New(value: Guid.NewGuid()), budgetId: budgetId,
            budgetCode: budgetCode, ownerId: ownerId, participantId: personId, permissionType: permissionType,
            status: BudgetPermissionRequestStatus.Pending, expirationDate: expirationDate,
            submissionDate: submissionDate);
    }

    public void Confirm(DateAndTime confirmationDate)
    {
        StatusTracker.Confirm(confirmationDate: confirmationDate);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestConfirmedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            BudgetCode: BudgetCode, PermissionType: PermissionType));
    }

    public void Cancel(DateAndTime cancellationDate)
    {
        StatusTracker.Cancel(cancellationDate: cancellationDate);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestCancelledEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            BudgetCode: BudgetCode, PermissionType: PermissionType));
    }

    public void Expire()
    {
        StatusTracker.Expire();

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRequestExpiredEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: ParticipantId,
            BudgetCode: BudgetCode, PermissionType: PermissionType, ExpirationDate: StatusTracker.ExpirationDate));
    }
}