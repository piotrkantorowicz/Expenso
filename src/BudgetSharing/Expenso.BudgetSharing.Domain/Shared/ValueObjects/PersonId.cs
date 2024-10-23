using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.ValueObjects;

public sealed record PersonId
{
    private PersonId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PersonId New(Guid? value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new EmptyIdentifierCannotBeProcessed(identifier: value, type: typeof(PersonId)))
        ]);

        return new PersonId(value: value!.Value);
    }

    public static PersonId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new PersonId(value: value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}