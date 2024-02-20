using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    FinancePreferenceContract FinancePreference) : IIntegrationEvent;