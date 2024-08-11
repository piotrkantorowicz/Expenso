namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceResponse_FinancePreference? FinancePreference,
    GetPreferenceResponse_NotificationPreference? NotificationPreference,
    GetPreferenceResponse_GeneralPreference? GeneralPreference);