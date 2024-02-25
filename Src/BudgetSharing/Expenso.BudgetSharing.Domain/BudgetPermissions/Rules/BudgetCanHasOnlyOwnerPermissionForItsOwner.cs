using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetCanHasOnlyOwnerPermissionForItsOwner(
    BudgetId budgetId,
    PersonId participantId,
    PersonId ownerId,
    PermissionType permissionType) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PersonId _ownerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
    private readonly PersonId _participantId = participantId ?? throw new ArgumentNullException(nameof(participantId));

    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(nameof(permissionType));

    public string Message =>
        $"Budget {_budgetId} cannot have owner permission for other user {_participantId} that its owner {_ownerId}";

    public bool IsBroken()
    {
        return _permissionType == PermissionType.Owner && _ownerId != _participantId;
    }
}