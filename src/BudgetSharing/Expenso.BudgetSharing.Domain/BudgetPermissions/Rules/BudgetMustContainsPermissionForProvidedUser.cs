using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetMustContainsPermissionForProvidedUser : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly PersonId _participantId;
    private readonly Permission? _permission;

    public BudgetMustContainsPermissionForProvidedUser(BudgetId budgetId, PersonId participantId,
        Permission? permission)
    {
        _permission = permission;
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
        _participantId = participantId ?? throw new ArgumentNullException(paramName: nameof(participantId));
    }

    public string Message =>
        $"Budget with ID {_budgetId} does not have permission for provided user with ID {_participantId}.";

    public bool IsBroken()
    {
        return _permission is null;
    }
}