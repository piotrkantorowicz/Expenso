namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdatePreferenceRequest(
    UpdatePreferenceRequestFinancePreference? FinancePreference,
    UpdatePreferenceRequestNotificationPreference? NotificationPreference,
    UpdatePreferenceRequestGeneralPreference? GeneralPreference);