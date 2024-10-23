using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.GeneralPreferences.Payload;

namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.GeneralPreferences;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    GeneralPreferenceUpdatedPayload Payload) : IIntegrationEvent;