using Expenso.Shared.IntegrationEvents;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(
    Guid UserId,
    GeneralPreferenceInternalContract GeneralPreference) : IIntegrationEvent;