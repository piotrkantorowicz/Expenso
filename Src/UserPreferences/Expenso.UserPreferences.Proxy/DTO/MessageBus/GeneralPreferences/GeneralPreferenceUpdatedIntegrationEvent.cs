using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;

public sealed record GeneralPreferenceUpdatedIntegrationEvent(
    IMessageContext MessageContext,
    Guid UserId,
    GeneralPreferenceContract GeneralPreference) : IIntegrationEvent;