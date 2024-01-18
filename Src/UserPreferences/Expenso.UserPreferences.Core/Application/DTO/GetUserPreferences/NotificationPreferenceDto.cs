namespace Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;

public sealed record NotificationPreferenceDto(bool SendFinanceReportEnabled, int SendFinanceReportInterval);