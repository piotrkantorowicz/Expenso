namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

public sealed record GetPreferenceResponseNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);