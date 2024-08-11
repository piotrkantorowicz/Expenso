namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdatePreferenceRequest(
    UpdatePreferenceRequest_FinancePreference? FinancePreference,
    UpdatePreferenceRequest_NotificationPreference? NotificationPreference,
    UpdatePreferenceRequest_GeneralPreference? GeneralPreference);