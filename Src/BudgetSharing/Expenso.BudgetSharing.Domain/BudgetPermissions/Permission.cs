using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions;

public class Permission
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private Permission()
    {
        ParticipantId = default!;
        PermissionType = default!;
    }

    private Permission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules([
            new UnknownPermissionTypeCannotBeProcessed(permissionType)
        ]);

        ParticipantId = participantId;
        PermissionType = permissionType;
    }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    internal static Permission Create(PersonId participantId, PermissionType permissionType)
    {
        return new Permission(participantId, permissionType);
    }
}