using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.ValueObjects;

public sealed record BudgetCode
{
    private BudgetCode(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static BudgetCode New(string? value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(BusinessRule: new BudgetCodeMustHasCorrectFormat(budgetCode: value!))
        ]);

        return new BudgetCode(value: value!);
    }

    public static BudgetCode? Nullable(string? value)
    {
        return value is null ? null : New(value: value.Trim());
    }

    public override string ToString()
    {
        return Value;
    }
}