using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;

internal sealed class BudgetMustHasDistinctPermissionsForUsersAndTypes(
    BudgetId budgetId,
    PersonId participantId,
    PermissionType permissionType,
    IEnumerable<Permission> permissions) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PersonId _participantId = participantId ?? throw new ArgumentNullException(nameof(participantId));

    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(nameof(permissionType));

    private readonly IEnumerable<Permission> _permissions =
        permissions ?? throw new ArgumentNullException(nameof(permissions));

    public string Message => new StringBuilder()
        .Append("Budget ")
        .Append(_budgetId)
        .Append(" already has permission ")
        .Append(_permissionType)
        .Append(" for participant ")
        .Append(_participantId)
        .ToString();

    public bool IsBroken()
    {
        return _permissions.SingleOrDefault(x =>
            x.ParticipantId == _participantId && x.PermissionType == _permissionType) is not null;
    }
}