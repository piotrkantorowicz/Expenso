using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.FinancePreferences.Payload;

namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    FinancePreferenceUpdatedPayload Payload) : IIntegrationEvent;