namespace Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

public sealed record CreateNotificationPreferenceResponse(bool SendFinanceReportEnabled, int SendFinanceReportInterval);