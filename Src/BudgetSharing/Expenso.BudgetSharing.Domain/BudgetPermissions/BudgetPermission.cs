using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions;

public class BudgetPermission : IAggregateRoot
{
    private readonly DomainEventsSource _domainEventsSource = new();

    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private BudgetPermission()
    {
        Id = default!;
        BudgetId = default!;
        OwnerId = default!;
        Permissions = new List<Permission>();
    }

    private BudgetPermission(BudgetPermissionId id, BudgetId budgetId, PersonId ownerId)
    {
        Id = id;
        BudgetId = budgetId;
        OwnerId = ownerId;
        Permissions = new List<Permission>();
    }

    public BudgetPermissionId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId OwnerId { get; }

    public ICollection<Permission> Permissions { get; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.DomainEvents;
    }

    public static BudgetPermission Create(BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(BudgetPermissionId.New(Guid.NewGuid()), budgetId, ownerId);
    }

    public void AddPermission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules([
            new BudgetMustHasDistinctPermissionsForUsersAndTypes(BudgetId, participantId, permissionType, Permissions),
            new BudgetCanHasOnlyOneOwnerPermission(BudgetId, Permissions),
            new BudgetCanHasOnlyOwnerPermissionForItsOwner(BudgetId, participantId, OwnerId, permissionType)
        ]);

        Permissions.Add(Permission.Create(participantId, permissionType));
        _domainEventsSource.AddDomainEvent(new BudgetPermissionGrantedEvent(BudgetId, participantId, permissionType));
    }

    public void RemovePermission(PersonId participantId)
    {
        Permission? permission = Permissions.SingleOrDefault(x => x.ParticipantId == participantId);

        DomainModelState.CheckBusinessRules([
            new BudgetMustContainsPermissionForProvidedUser(BudgetId, participantId, permission)
        ]);

        Permission validatedPermission = permission!;
        Permissions.Remove(validatedPermission);

        _domainEventsSource.AddDomainEvent(new BudgetPermissionWithdrawnEvent(BudgetId,
            validatedPermission.ParticipantId, validatedPermission.PermissionType));
    }
}