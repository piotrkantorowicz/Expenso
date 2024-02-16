namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

public sealed record GetPreferenceResponse(
    GetFinancePreferenceResponse? FinancePreference,
    GetNotificationPreferenceResponse? NotificationPreference,
    GetGeneralPreferenceResponse? GeneralPreference);