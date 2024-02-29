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
    // Required by EF Core   
    private BudgetPermission()
    {
        Id = default!;
        BudgetId = default!;
        OwnerId = default!;
        _domainEventsSource = default!;
        _messageContextFactory = default!;
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
        return new BudgetPermission(budgetPermissionId, budgetId, ownerId);
    }

    public static BudgetPermission Create(BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(BudgetPermissionId.New(Guid.NewGuid()), budgetId, ownerId);
    }

    public void AddPermission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules([
            (new BudgetMustHasDistinctPermissionsForUsers(BudgetId, participantId, Permissions), false),
            (new BudgetCanHasOnlyOneOwnerPermission(BudgetId, permissionType, Permissions), true),
            (new BudgetCanHasOnlyOwnerPermissionForItsOwner(BudgetId, participantId, OwnerId, permissionType), true)
        ]);

        Permissions.Add(Permission.Create(participantId, permissionType));

        _domainEventsSource.AddDomainEvent(new BudgetPermissionGrantedEvent(_messageContextFactory.Current(), Id,
            BudgetId, participantId, permissionType));
    }

    public void RemovePermission(PersonId participantId)
    {
        Permission? permission = Permissions.SingleOrDefault(x => x.ParticipantId == participantId);

        DomainModelState.CheckBusinessRules([
            (new BudgetMustContainsPermissionForProvidedUser(BudgetId, participantId, permission), true),
            (new OwnerPermissionCannotBeRemoved(BudgetId, permission?.PermissionType), true)
        ]);

        Permissions.Remove(permission ?? throw new ArgumentException(nameof(permission)));

        _domainEventsSource.AddDomainEvent(new BudgetPermissionWithdrawnEvent(_messageContextFactory.Current(), Id,
            BudgetId, permission.ParticipantId, permission.PermissionType));
    }

    public void Delete(IClock? clock = null)
    {
        DomainModelState.CheckBusinessRules([
            (new BudgetPermissionCannotBeDeletedIfItIsAlreadyDeleted(Id, Deletion), false)
        ]);

        Deletion = SafeDeletion.Delete(clock?.UtcNow ?? DateTimeOffset.UtcNow);

        _domainEventsSource.AddDomainEvent(new BudgetPermissionDeletedEvent(_messageContextFactory.Current(), Id,
            BudgetId, Permissions.Select(x => x.ParticipantId).ToList().AsReadOnly()));
    }

    public void Restore()
    {
        DomainModelState.CheckBusinessRules([
            (new BudgetPermissionCannotBeRestoredIfItIsNotRemoved(Id, Deletion), false)
        ]);

        Deletion = null;

        _domainEventsSource.AddDomainEvent(new BudgetPermissionRestoredEvent(_messageContextFactory.Current(), Id,
            BudgetId, Permissions.Select(x => x.ParticipantId).ToList().AsReadOnly()));
    }
}