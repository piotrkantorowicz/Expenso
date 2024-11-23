namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceResponseFinancePreference? FinancePreference,
    GetPreferenceResponseNotificationPreference? NotificationPreference,
    GetPreferenceResponseGeneralPreference? GeneralPreference);