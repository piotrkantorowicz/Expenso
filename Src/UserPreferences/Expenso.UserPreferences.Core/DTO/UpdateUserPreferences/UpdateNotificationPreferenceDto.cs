namespace Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;

public sealed record UpdateNotificationPreferenceDto(bool SendFinanceReportEnabled, int SendFinanceReportInterval);