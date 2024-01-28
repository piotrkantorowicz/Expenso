using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

public sealed record BudgetPermission
{
    private readonly DomainModelState _domainModelState = new();

    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private BudgetPermission() : this(BudgetPermissionId.CreateDefault(), BudgetId.CreateDefault(),
        PersonId.CreateDefault())
    {
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

    public static BudgetPermission Create(BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(Guid.NewGuid(), budgetId, ownerId);
    }

    public void AddPermission(PersonId participantId, PermissionType permissionType)
    {
        _domainModelState.CheckBusinessRules([
            new BudgetMustHasDistinctPermissionsForUsersAndTypes(BudgetId, participantId, permissionType, Permissions),
            new BudgetCanHasOnlyOneOwnerPermission(BudgetId, Permissions),
            new BudgetCanHasOnlyOwnerPermissionForItsOwner(BudgetId, participantId, OwnerId, permissionType)
        ]);

        Permissions.Add(Permission.Create(Id, participantId, permissionType));
    }

    public void RemovePermission(PersonId participantId, PermissionType permissionType)
    {
        Permission? permission =
            Permissions.SingleOrDefault(x => x.ParticipantId == participantId && x.PermissionType == permissionType);

        _domainModelState.CheckBusinessRules([
            new BudgetMustContainsPermissionForProvidedUsersAndTypes(BudgetId, participantId, permissionType,
                Permissions)
        ]);

        Permissions.Remove(permission!);
    }
}
