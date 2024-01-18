namespace Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;

public sealed record PreferenceDto(
    Guid PreferenceId,
    Guid UserId,
    FinancePreferenceDto FinancePreference,
    NotificationPreferenceDto NotificationPreference,
    GeneralPreferenceDto GeneralPreference);