using Expenso.Shared.IntegrationEvents;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    Guid UserId,
    NotificationPreferenceContract NotificationPreference) : IIntegrationEvent;