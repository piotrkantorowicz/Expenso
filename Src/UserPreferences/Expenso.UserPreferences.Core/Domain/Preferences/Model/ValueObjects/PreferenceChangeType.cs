namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record PreferenceChangeType
{
    private PreferenceChangeType(bool generalPreferencesChanged, bool financePreferencesChanged,
        bool notificationPreferencesChanged)
    {
        GeneralPreferencesChanged = generalPreferencesChanged;
        FinancePreferencesChanged = financePreferencesChanged;
        NotificationPreferencesChanged = notificationPreferencesChanged;
    }

    public bool GeneralPreferencesChanged { get; }

    public bool FinancePreferencesChanged { get; }

    public bool NotificationPreferencesChanged { get; }

    public bool IsAnyPreferencesChanged =>
        GeneralPreferencesChanged || FinancePreferencesChanged || NotificationPreferencesChanged;

    public static PreferenceChangeType Create(bool generalPreferencesChanged, bool financePreferencesChanged,
        bool notificationPreferencesChanged)
    {
        return new PreferenceChangeType(generalPreferencesChanged, financePreferencesChanged,
            notificationPreferencesChanged);
    }
}