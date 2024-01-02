using Expenso.Shared.IntegrationEvents;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Proxy.IntegrationEvents;

public sealed record FinancePreferenceUpdatedIntegrationEvent(Guid UserId, FinancePreferenceContract FinancePreference)
    : IIntegrationEvent;