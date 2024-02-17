namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdatePreferenceRequest(
    UpdateFinancePreferenceRequest? FinancePreference,
    UpdateNotificationPreferenceRequest? NotificationPreference,
    UpdateGeneralPreferenceRequest? GeneralPreference);