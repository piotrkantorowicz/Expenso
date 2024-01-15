namespace Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

public sealed record PreferenceContract(
    Guid PreferenceId,
    Guid UserId,
    FinancePreferenceContract FinancePreference,
    NotificationPreferenceContract NotificationPreference,
    GeneralPreferenceContract GeneralPreference);