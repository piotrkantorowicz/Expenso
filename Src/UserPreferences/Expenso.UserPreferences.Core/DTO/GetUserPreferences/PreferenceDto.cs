namespace Expenso.UserPreferences.Core.DTO.GetUserPreferences;

public sealed record PreferenceDto(
    Guid PreferencesId,
    Guid UserId,
    FinancePreferenceDto FinancePreference,
    NotificationPreferenceDto NotificationPreference,
    GeneralPreferenceDto GeneralPreference);