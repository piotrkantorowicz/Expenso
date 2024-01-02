using Expenso.Shared.IntegrationEvents;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Proxy.IntegrationEvents;

public sealed record NotificationPreferenceUpdatedIntegrationEvent(
    Guid UserId,
    NotificationPreferenceContract NotificationPreference) : IIntegrationEvent;