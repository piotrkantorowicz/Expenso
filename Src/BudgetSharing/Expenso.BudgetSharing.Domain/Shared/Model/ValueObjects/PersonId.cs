namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed class PersonId
{
    private PersonId(Guid value)
    {
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

    public static PersonId CreateDefault()
    {
        return new PersonId(Guid.Empty);
    }

    public static PersonId Create(Guid value)
    {
        return new PersonId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}
