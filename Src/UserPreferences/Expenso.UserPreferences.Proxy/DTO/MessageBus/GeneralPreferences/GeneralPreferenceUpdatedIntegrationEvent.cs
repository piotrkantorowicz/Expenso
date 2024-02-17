using Expenso.Shared.Integration.Events;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(Guid UserId, GeneralPreferenceContract GeneralPreference)
    : IIntegrationEvent;