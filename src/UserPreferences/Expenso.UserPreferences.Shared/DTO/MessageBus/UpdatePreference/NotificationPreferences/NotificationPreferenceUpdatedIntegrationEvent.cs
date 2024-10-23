using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences.Payload;

namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    NotificationPreferenceUpdatedPayload Payload) : IIntegrationEvent;