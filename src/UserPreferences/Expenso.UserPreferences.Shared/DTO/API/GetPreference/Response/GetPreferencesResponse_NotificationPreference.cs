namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponse_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);