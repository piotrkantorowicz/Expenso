using Expenso.Shared.IntegrationEvents;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent(
    Guid UserId,
    FinancePreferenceInternalContract FinancePreference)
    : IIntegrationEvent;