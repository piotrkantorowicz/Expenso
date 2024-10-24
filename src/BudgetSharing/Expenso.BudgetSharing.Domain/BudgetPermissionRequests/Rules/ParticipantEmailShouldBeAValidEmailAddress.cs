using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ParticipantEmailShouldBeAValidEmailAddress : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly string? _email;

    public ParticipantEmailShouldBeAValidEmailAddress(string? email, BudgetId budgetId)
    {
        _email = email;
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
    }

    public string Message =>
        $"Participant email {_email} should be a valid email address. Budget {_budgetId}.";

    public bool IsBroken()
    {
        return _email.IsValidEmail() is false;
    }
}