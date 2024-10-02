namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

public sealed record UpdatePreferenceRequest(
    UpdatePreferenceRequestFinancePreference? FinancePreference,
    UpdatePreferenceRequestNotificationPreference? NotificationPreference,
    UpdatePreferenceRequestGeneralPreference? GeneralPreference);