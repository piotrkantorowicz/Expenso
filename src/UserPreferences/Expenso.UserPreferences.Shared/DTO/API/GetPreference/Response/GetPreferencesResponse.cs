namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponse(
    Guid Id,
    Guid UserId,
    GetPreferencesResponseFinancePreference? FinancePreference,
    GetPreferencesResponseNotificationPreference? NotificationPreference,
    GetPreferencesResponseGeneralPreference? GeneralPreference);