namespace Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;

public sealed record UpdatePreferenceDto(
    UpdateFinancePreferenceDto? FinancePreference,
    UpdateNotificationPreferenceDto? NotificationPreference,
    UpdateGeneralPreferenceDto? GeneralPreference);