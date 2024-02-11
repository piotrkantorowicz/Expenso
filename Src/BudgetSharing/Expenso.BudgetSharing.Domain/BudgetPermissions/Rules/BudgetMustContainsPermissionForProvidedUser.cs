using System.Text;

using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

public class BudgetMustContainsPermissionForProvidedUser(
    BudgetId budgetId,
    PersonId participantId,
    Permission? permission) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PersonId _participantId = participantId ?? throw new ArgumentNullException(nameof(participantId));

    public string Message =>
        new StringBuilder()
            .Append("Budget with id: ")
            .Append(_budgetId)
            .Append(" does not have permission for provided user with id: ")
            .Append(_participantId)
            .ToString();

    public bool IsBroken()
    {
        return permission is null;
    }
}