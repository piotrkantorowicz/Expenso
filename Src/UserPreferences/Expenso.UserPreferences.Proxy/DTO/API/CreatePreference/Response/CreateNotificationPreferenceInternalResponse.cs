namespace Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

public sealed record CreateNotificationPreferenceInternalResponse(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);