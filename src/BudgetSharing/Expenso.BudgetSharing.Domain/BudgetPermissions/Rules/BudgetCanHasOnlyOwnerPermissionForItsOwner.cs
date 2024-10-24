using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetCanHasOnlyOwnerPermissionForItsOwner : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly PersonId _ownerId;
    private readonly PersonId _participantId;
    private readonly PermissionType _permissionType;

    public BudgetCanHasOnlyOwnerPermissionForItsOwner(BudgetId budgetId, PersonId participantId, PersonId ownerId,
        PermissionType permissionType)
    {
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
        _ownerId = ownerId ?? throw new ArgumentNullException(paramName: nameof(ownerId));
        _participantId = participantId ?? throw new ArgumentNullException(paramName: nameof(participantId));
        _permissionType = permissionType ?? throw new ArgumentNullException(paramName: nameof(permissionType));
    }

    public string Message =>
        $"Budget {_budgetId} cannot have owner permission for other user {_participantId} that its owner {_ownerId}.";

    public bool IsBroken()
    {
        return _permissionType == PermissionType.Owner && _ownerId != _participantId;
    }
}