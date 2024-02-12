namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record PreferenceId
{
    private PreferenceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PreferenceId New(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new PreferenceId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}