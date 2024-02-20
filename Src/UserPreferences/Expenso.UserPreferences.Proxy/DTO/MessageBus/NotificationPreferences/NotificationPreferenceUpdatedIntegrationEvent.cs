using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    NotificationPreferenceContract NotificationPreference) : IIntegrationEvent;