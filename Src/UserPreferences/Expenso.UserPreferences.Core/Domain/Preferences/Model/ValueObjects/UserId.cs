namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record UserId
{
    private UserId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(UserId id) => id.Value;

    public static implicit operator UserId(Guid id)
    {
        return new UserId(id);
    }

    public static UserId CreateDefault()
    {
        return new UserId(Guid.Empty);
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }

    public Guid Value { get; }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}