namespace Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

public sealed record PreferenceContract(
    Guid PreferencesId,
    Guid UserId,
    FinancePreferenceContract FinancePreferenceContract,
    NotificationPreferenceContract NotificationPreferenceContract,
    GeneralPreferenceContract GeneralPreferenceContract);