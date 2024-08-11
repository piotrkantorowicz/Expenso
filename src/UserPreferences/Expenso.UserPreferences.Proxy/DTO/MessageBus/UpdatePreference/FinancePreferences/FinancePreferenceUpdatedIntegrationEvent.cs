using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    FinancePreferenceUpdatedIntegrationEvent_FinancePreference FinancePreference) : IIntegrationEvent;