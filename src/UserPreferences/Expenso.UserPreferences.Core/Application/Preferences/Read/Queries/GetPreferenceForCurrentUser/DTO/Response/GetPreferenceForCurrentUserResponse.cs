namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

public sealed record GetPreferenceForCurrentUserResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceForCurrentUserResponseFinancePreference? FinancePreference,
    GetPreferenceForCurrentUserResponseNotificationPreference? NotificationPreference,
    GetPreferenceForCurrentUserResponseGeneralPreference? GeneralPreference);