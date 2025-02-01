using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

public sealed record UpdatePreferenceCommand(
    IMessageContext MessageContext,
    Guid PreferenceId,
    UpdatePreferenceRequest? Payload) : ICommand;