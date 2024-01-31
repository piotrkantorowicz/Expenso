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

    internal static PersonId CreateDefault()
    {
        return new PersonId(Guid.Empty);
    }

    public static PersonId Create(Guid value)
    {
        return new PersonId(value);
    }
}