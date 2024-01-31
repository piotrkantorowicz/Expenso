using System.Text;

using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;

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
        new StringBuilder()
            .Append("Budget ")
            .Append(_budgetId)
            .Append(" cannot have owner permission for other user ")
            .Append(_participantId)
            .Append(" that its owner ")
            .Append(_ownerId)
            .ToString();

    public bool IsBroken()
    {
        return _permissionType == PermissionType.Owner && _ownerId != _participantId;
    }
}