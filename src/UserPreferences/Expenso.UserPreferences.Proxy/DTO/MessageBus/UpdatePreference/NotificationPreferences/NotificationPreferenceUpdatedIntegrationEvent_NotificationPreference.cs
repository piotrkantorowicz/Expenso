namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);