using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

public sealed class Permission
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private Permission()
    {
        Id = PermissionId.CreateDefault();
        BudgetPermissionId = BudgetPermissionId.CreateDefault();
        ParticipantId = PersonId.CreateDefault();
        PermissionType = PermissionType.Unknown;
    }

    private Permission(PermissionId id, BudgetPermissionId budgetPermissionId, PersonId participantId,
        PermissionType permissionType, DomainModelState domainModelState)
    {
        domainModelState.CheckBusinessRules([
            new UnknownPermissionTypeCannotBeProcessed(permissionType)
        ]);

        Id = id;
        BudgetPermissionId = budgetPermissionId;
        ParticipantId = participantId;
        PermissionType = permissionType;
    }

    public PermissionId Id { get; }

    public BudgetPermissionId BudgetPermissionId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    internal static Permission Create(BudgetPermissionId budgetPermissionId, PersonId participantId,
        PermissionType permissionType, DomainModelState domainModelState)
    {
        return new Permission(Guid.NewGuid(), budgetPermissionId, participantId, permissionType, domainModelState);
    }
}