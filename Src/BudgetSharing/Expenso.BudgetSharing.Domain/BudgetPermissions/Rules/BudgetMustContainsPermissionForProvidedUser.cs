using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetMustContainsPermissionForProvidedUser(
    BudgetId budgetId,
    PersonId participantId,
    Permission? permission) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PersonId _participantId = participantId ?? throw new ArgumentNullException(nameof(participantId));

    public string Message =>
        $"Budget with id: {_budgetId} does not have permission for provided user with id: {_participantId}";

    public bool IsBroken()
    {
        return permission is null;
    }
}