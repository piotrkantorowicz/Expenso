namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceResponseFinancePreference? FinancePreference,
    GetPreferenceResponseNotificationPreference? NotificationPreference,
    GetPreferenceResponseGeneralPreference? GeneralPreference);