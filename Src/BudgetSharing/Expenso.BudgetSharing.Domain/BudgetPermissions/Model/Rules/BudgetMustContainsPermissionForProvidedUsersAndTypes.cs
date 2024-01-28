using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;

public class BudgetMustContainsPermissionForProvidedUsersAndTypes(
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

    public string Message =>
        $"Budget {_budgetId} does not have permission {_permissionType} for participant {_participantId}";

    public bool IsBroken()
    {
        return _permissions.SingleOrDefault(x =>
            x.ParticipantId == _participantId && x.PermissionType == _permissionType) is null;
    }
}
