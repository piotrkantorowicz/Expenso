using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.BudgetSharing.Domain.Shared.Base;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions;

public sealed class BudgetPermission : IAggregateRoot
{
    private readonly DomainEventsSource _domainEventsSource;
    private readonly IMessageContextFactory _messageContextFactory;

    // ReSharper disable once UnusedMember.Local
    // Required for EF Core
    private BudgetPermission()
    {
        Id = default!;
        BudgetId = default!;
        OwnerId = default!;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
        Permissions = new List<Permission>();
    }

    private BudgetPermission(BudgetPermissionId id, BudgetId budgetId, PersonId ownerId)
    {
        Id = id;
        BudgetId = budgetId;
        OwnerId = ownerId;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
        Permissions = new List<Permission>();
    }

    public BudgetPermissionId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId OwnerId { get; }

    public ICollection<Permission> Permissions { get; }

    public SafeDeletion? Deletion { get; private set; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.DomainEvents;
    }

    public static BudgetPermission Create(BudgetPermissionId budgetPermissionId, BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(id: budgetPermissionId, budgetId: budgetId, ownerId: ownerId);
    }

    public static BudgetPermission Create(BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(id: BudgetPermissionId.New(value: Guid.NewGuid()), budgetId: budgetId,
            ownerId: ownerId);
    }

    public void AddPermission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new BudgetMustHasDistinctPermissionsForUsers(budgetId: BudgetId, participantId: participantId, permissions: Permissions),
                false),
            (new BudgetCanHasOnlyOneOwnerPermission(budgetId: BudgetId, permissionType: permissionType, permissions: Permissions),
                true),
            (new BudgetCanHasOnlyOwnerPermissionForItsOwner(budgetId: BudgetId, participantId: participantId, ownerId: OwnerId, permissionType: permissionType),
                true)
        ]);

        Permissions.Add(item: Permission.Create(participantId: participantId, permissionType: permissionType));

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionGrantedEvent(
            MessageContext: _messageContextFactory.Current(), BudgetPermissionId: Id, BudgetId: BudgetId,
            ParticipantId: participantId, PermissionType: permissionType));
    }

    public void RemovePermission(PersonId participantId)
    {
        Permission? permission = Permissions.SingleOrDefault(predicate: x => x.ParticipantId == participantId);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new BudgetMustContainsPermissionForProvidedUser(budgetId: BudgetId, participantId: participantId, permission: permission),
                true),
            (new OwnerPermissionCannotBeRemoved(budgetId: BudgetId, permissionType: permission?.PermissionType), true)
        ]);

        Permissions.Remove(item: permission ?? throw new ArgumentException(message: nameof(permission)));

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionWithdrawnEvent(
            MessageContext: _messageContextFactory.Current(), BudgetPermissionId: Id, BudgetId: BudgetId,
            ParticipantId: permission.ParticipantId, PermissionType: permission.PermissionType));
    }

    public void Delete(IClock? clock = null)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new BudgetPermissionCannotBeDeletedIfItIsAlreadyDeleted(budgetPermissionId: Id, removalInfo: Deletion),
                false)
        ]);

        Deletion = SafeDeletion.Delete(dateTime: clock?.UtcNow ?? DateTimeOffset.UtcNow);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionDeletedEvent(
            MessageContext: _messageContextFactory.Current(), BudgetPermissionId: Id, BudgetId: BudgetId,
            ParticipantIds: Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly()));
    }

    public void Restore()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new BudgetPermissionCannotBeRestoredIfItIsNotRemoved(budgetPermissionId: Id, removalInfo: Deletion), false)
        ]);

        Deletion = null;

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionRestoredEvent(
            MessageContext: _messageContextFactory.Current(), BudgetPermissionId: Id, BudgetId: BudgetId,
            ParticipantIds: Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly()));
    }
}