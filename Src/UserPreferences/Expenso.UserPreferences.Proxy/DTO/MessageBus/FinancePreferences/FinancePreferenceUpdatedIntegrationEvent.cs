using Expenso.Shared.Integration.Events;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent(Guid UserId, FinancePreferenceContract FinancePreference)
    : IIntegrationEvent;