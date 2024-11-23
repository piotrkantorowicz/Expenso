namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

public sealed record GetPreferenceForCurrentUserResponseNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);