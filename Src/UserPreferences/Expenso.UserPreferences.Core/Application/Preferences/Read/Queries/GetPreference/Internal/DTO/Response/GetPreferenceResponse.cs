namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetFinancePreferenceResponse? FinancePreference,
    GetNotificationPreferenceResponse? NotificationPreference,
    GetGeneralPreferenceResponse? GeneralPreference);