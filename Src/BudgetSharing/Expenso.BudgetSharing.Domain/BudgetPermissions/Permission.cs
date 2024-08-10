using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Model;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions;

public sealed class Permission
{
    // ReSharper disable once UnusedMember.Local
    // Required for EF Core
    private Permission()
    {
        ParticipantId = default!;
        PermissionType = default!;
    }

    private Permission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new UnknownPermissionTypeCannotBeProcessed(permissionType: permissionType), false)
        ]);

        ParticipantId = participantId;
        PermissionType = permissionType;
    }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    internal static Permission Create(PersonId participantId, PermissionType permissionType)
    {
        return new Permission(participantId: participantId, permissionType: permissionType);
    }
}