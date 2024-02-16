namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;

public sealed record GetNotificationPreferenceResponse(bool SendFinanceReportEnabled, int SendFinanceReportInterval);