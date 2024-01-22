namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

public sealed record UpdatePreferenceRequest(
    UpdateFinancePreferenceRequest? FinancePreference,
    UpdateNotificationPreferenceRequest? NotificationPreference,
    UpdateGeneralPreferenceRequest? GeneralPreference);