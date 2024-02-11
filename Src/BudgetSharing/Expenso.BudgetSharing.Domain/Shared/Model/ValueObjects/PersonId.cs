using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed class PersonId
{
    private PersonId(Guid value)
    {
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(PersonId id)
    {
        return id.Value;
    }

    public static implicit operator PersonId(Guid id)
    {
        return new PersonId(id);
    }

    public static PersonId Create(Guid value)
    {
        return new PersonId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}