namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetFinancePreferenceResponse FinancePreference,
    GetNotificationPreferenceResponse NotificationPreference,
    GetGeneralPreferenceResponse GeneralPreference);