namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record UserId
{
    private UserId()
    {
        Value = Guid.Empty;
    }
    
    private UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(nameof(value));
        
        Value = value;
    }

    public Guid Value { get; }

    public static UserId Default()
    {
        return new UserId();
    }

    public static UserId New(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(nameof(value));
        
        return new UserId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}