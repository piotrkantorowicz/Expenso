namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

public sealed record UpdateNotificationPreferenceRequest(bool SendFinanceReportEnabled, int SendFinanceReportInterval);