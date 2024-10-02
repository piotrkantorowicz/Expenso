namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

public sealed record GetPreferenceResponseNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);