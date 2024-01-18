namespace Expenso.UserPreferences.Core.Application.DTO.UpdateUserPreferences;

public sealed record UpdatePreferenceDto(
    UpdateFinancePreferenceDto? FinancePreference,
    UpdateNotificationPreferenceDto? NotificationPreference,
    UpdateGeneralPreferenceDto? GeneralPreference);