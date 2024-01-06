using Expenso.Shared.IntegrationEvents;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Proxy.IntegrationEvents;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(Guid UserId, GeneralPreferenceContract GeneralPreference)
    : IIntegrationEvent;