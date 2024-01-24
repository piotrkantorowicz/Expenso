namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record PreferenceChangeType(
    bool GeneralPreferencesChanged,
    bool FinancePreferencesChanged,
    bool NotificationPreferencesChanged)
{
    public bool IsAnyPreferencesChanged =>
        GeneralPreferencesChanged || FinancePreferencesChanged || NotificationPreferencesChanged;
}