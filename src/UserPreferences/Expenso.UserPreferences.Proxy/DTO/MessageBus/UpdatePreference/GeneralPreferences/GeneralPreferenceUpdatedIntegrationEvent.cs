using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.GeneralPreferences;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    GeneralPreferenceUpdatedIntegrationEventGeneralPreference GeneralPreference) : IIntegrationEvent;