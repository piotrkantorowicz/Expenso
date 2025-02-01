namespace Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

public sealed record UpdatePreferenceRequest(
    UpdatePreferenceRequestFinancePreference? FinancePreference,
    UpdatePreferenceRequestNotificationPreference? NotificationPreference,
    UpdatePreferenceRequestGeneralPreference? GeneralPreference);