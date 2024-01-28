using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

public sealed class Permission
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private Permission() : this(PermissionId.CreateDefault(), BudgetPermissionId.CreateDefault(),
        PersonId.CreateDefault(), PermissionType.Unknown)
    {
    }

    private Permission(PermissionId id, BudgetPermissionId budgetPermissionId, PersonId participantId,
        PermissionType permissionType)
    {
        Id = id;
        BudgetPermissionId = budgetPermissionId;
        ParticipantId = participantId;
        PermissionType = permissionType;
    }

    public PermissionId Id { get; }

    public BudgetPermissionId BudgetPermissionId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public static Permission Create(BudgetPermissionId budgetPermissionId, PersonId participantId,
        PermissionType permissionType)
    {
        return new Permission(Guid.NewGuid(), budgetPermissionId, participantId, permissionType);
    }
}
