namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdateNotificationPreferenceRequest(bool SendFinanceReportEnabled, int SendFinanceReportInterval);