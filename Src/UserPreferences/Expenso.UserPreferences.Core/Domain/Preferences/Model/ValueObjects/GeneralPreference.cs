namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record GeneralPreference
{
    private GeneralPreference() : this(false)
    {
    }

    private GeneralPreference(bool useDarkMode)
    {
        UseDarkMode = useDarkMode;
    }

    public bool UseDarkMode { get; }

    public static GeneralPreference CreateDefault()
    {
        return new GeneralPreference();
    }

    public static GeneralPreference Create(bool isDarkModeEnabled)
    {
        return new GeneralPreference(isDarkModeEnabled);
    }
}