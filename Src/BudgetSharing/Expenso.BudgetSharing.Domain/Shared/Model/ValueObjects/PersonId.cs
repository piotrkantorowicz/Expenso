using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed record PersonId
{
    private PersonId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PersonId New(Guid value)
    {
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, typeof(PersonId))]);

        return new PersonId(value);
    }

    public static PersonId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new PersonId(value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}