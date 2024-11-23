namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponseNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);