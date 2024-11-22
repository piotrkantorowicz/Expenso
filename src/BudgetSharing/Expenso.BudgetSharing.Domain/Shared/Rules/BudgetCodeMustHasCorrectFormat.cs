using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.BudgetSharing.Domain.Shared.Rules;

public sealed class BudgetCodeMustHasCorrectFormat : IBusinessRule
{
    private readonly string? _budgetCode;

    public BudgetCodeMustHasCorrectFormat(string? budgetCode)
    {
        _budgetCode = budgetCode;
    }

    public string Message => $"Budget code {_budgetCode} must have correct format.";

    public bool IsBroken()
    {
        return !_budgetCode.IsValidBudgetCode();
    }
}