namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEventNotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);