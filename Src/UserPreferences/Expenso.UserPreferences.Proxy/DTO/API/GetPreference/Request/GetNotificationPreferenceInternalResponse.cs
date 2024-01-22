namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

public sealed record GetNotificationPreferenceInternalResponse(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);