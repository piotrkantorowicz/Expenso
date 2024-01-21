namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

public sealed record NotificationPreferenceInternalContract(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);