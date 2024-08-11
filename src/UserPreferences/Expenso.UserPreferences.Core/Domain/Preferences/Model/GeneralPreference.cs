namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record GeneralPreference
{
    public Guid Id { get; init; }

    public Guid PreferenceId { get; init; }

    public bool UseDarkMode { get; init; }
}