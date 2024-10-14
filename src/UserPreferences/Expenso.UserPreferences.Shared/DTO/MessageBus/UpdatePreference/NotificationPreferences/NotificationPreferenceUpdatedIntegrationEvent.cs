using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    NotificationPreferenceUpdatedIntegrationEvent_NotificationPreference NotificationPreference) : IIntegrationEvent;