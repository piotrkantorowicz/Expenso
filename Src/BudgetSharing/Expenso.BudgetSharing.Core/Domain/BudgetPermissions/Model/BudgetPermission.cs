using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.Shared;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;

internal sealed record BudgetPermission
{
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
        if (Permissions.Any(x => x.ParticipantId == participantId && x.PermissionType == permissionType))
        {
            throw new ArgumentException($"Permission for participant {participantId} already exists");
        }

        if (permissionType == PermissionType.Owner && Permissions.Any(x => x.PermissionType == PermissionType.Owner))
        {
            throw new ArgumentException("Budget can only have one owner");
        }

        if (permissionType == PermissionType.Owner && OwnerId != participantId)
        {
            throw new ArgumentException($"Only owner {OwnerId} can have owner permission");
        }

        Permissions.Add(Permission.Create(Id, participantId, permissionType));
    }

    public void RemovePermission(PersonId participantId, PermissionType permissionType)
    {
        Permission? permission =
            Permissions.SingleOrDefault(x => x.ParticipantId == participantId && x.PermissionType == permissionType);

        if (permission == null)
        {
            throw new ArgumentException($"Permission for participant {participantId} does not exist");
        }

        Permissions.Remove(permission);
    }
}