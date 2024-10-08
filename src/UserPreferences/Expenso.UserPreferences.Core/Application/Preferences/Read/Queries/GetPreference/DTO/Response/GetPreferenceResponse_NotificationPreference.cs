namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

public sealed record GetPreferenceResponse_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);