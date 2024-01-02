namespace Expenso.UserPreferences.Core.DTO.GetUserPreferences;

public sealed record NotificationPreferenceDto(bool SendFinanceReportEnabled, int SendFinanceReportInterval);