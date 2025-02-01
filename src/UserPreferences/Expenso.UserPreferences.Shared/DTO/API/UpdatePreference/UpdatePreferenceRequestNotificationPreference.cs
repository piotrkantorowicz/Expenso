namespace Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

public sealed record UpdatePreferenceRequestNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);