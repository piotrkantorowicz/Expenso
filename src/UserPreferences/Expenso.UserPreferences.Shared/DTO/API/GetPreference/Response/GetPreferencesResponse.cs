namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponse(
    Guid Id,
    Guid UserId,
    GetPreferencesResponse_FinancePreference? FinancePreference,
    GetPreferencesResponse_NotificationPreference? NotificationPreference,
    GetPreferencesResponse_GeneralPreference? GeneralPreference);