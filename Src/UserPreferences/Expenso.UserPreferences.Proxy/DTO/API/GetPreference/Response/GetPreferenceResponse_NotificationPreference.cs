namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

public sealed record GetPreferenceResponse_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);