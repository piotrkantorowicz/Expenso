namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

public sealed record GetNotificationPreferenceResponse(bool SendFinanceReportEnabled, int SendFinanceReportInterval);