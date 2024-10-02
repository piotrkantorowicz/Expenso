using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    NotificationPreferenceUpdatedIntegrationEventNotificationPreference NotificationPreference) : IIntegrationEvent;