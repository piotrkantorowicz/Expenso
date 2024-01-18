namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record PreferenceId
{
    private PreferenceId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(PreferenceId id) => id.Value;

    public static implicit operator PreferenceId(Guid id)
    {
        return new PreferenceId(id);
    }

    public static PreferenceId CreateDefault()
    {
        return new PreferenceId(Guid.Empty);
    }

    public static PreferenceId Create(Guid value)
    {
        return new PreferenceId(value);
    }

    public Guid Value { get; }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}