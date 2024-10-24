namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences.Payload;

public sealed record NotificationPreferenceUpdatedPayload(
    Guid UserId,
    bool SendFinanceReportEnabled,
    int SendFinanceReportInterval);