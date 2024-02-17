using Expenso.Shared.Integration.Events;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    Guid UserId,
    NotificationPreferenceContract NotificationPreference) : IIntegrationEvent;