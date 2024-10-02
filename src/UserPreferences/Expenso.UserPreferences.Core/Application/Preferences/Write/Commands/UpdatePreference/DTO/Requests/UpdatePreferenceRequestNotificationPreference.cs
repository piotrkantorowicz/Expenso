namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

public sealed record UpdatePreferenceRequestNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);