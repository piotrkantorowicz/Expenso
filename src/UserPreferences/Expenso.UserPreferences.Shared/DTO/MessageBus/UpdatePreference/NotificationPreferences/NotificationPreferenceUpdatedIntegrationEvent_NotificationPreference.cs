namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent_NotificationPreference(
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);