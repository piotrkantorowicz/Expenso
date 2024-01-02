namespace Expenso.UserPreferences.Core.DTO.GetUserPreferences;

public sealed record PreferenceDto(
    Guid PreferencesId,
    Guid UserId,
    FinancePreferenceDto FinancePreferenceDto,
    NotificationPreferenceDto NotificationPreferenceDto,
    GeneralPreferenceDto GeneralPreferenceDto);