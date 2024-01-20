namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

public sealed record GetPreferenceInternalResponse(
    GetFinancePreferenceInternalResponse? FinancePreference,
    GetNotificationPreferenceInternalResponse? NotificationPreference,
    GetGeneralPreferenceInternalResponse? GeneralPreference);