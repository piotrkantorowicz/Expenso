namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdatePreferenceRequest_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);