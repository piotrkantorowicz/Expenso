namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

public sealed record GetPreferenceResponse(
    Guid Id,
    Guid UserId,
    GetPreferenceResponse_FinancePreference? FinancePreference,
    GetPreferenceResponse_NotificationPreference? NotificationPreference,
    GetPreferenceResponse_GeneralPreference? GeneralPreference);