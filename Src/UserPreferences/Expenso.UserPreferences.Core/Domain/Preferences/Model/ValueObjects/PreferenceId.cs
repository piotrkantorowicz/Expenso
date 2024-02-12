namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record PreferenceId
{
    private PreferenceId()
    {
        Value = Guid.Empty;
    }
    
    private PreferenceId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(nameof(value));
        
        Value = value;
    }

    public Guid Value { get; }

    public static PreferenceId Default()
    {
        return new PreferenceId();
    }

    public static PreferenceId New(Guid value)
    {
        return new PreferenceId(value);
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