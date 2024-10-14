namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

public sealed record GetPreferenceForCurrentUserResponse_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);