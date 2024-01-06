namespace Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

public sealed record PreferenceContract(
    Guid PreferencesId,
    Guid UserId,
    FinancePreferenceContract FinancePreference,
    NotificationPreferenceContract NotificationPreference,
    GeneralPreferenceContract GeneralPreference);