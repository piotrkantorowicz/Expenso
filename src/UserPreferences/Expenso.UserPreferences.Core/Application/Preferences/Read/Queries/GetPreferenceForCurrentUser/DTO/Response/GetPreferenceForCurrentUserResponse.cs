namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

public sealed record GetPreferenceForCurrentUserResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceForCurrentUserResponse_FinancePreference? FinancePreference,
    GetPreferenceForCurrentUserResponse_NotificationPreference? NotificationPreference,
    GetPreferenceForCurrentUserResponse_GeneralPreference? GeneralPreference);