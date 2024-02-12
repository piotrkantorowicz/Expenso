namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record UserId
{
    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserId New(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(nameof(value));
        
        return new UserId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}