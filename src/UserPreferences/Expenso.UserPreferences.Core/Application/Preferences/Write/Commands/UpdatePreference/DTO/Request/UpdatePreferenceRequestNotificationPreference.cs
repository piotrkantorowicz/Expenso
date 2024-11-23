namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdatePreferenceRequestNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);