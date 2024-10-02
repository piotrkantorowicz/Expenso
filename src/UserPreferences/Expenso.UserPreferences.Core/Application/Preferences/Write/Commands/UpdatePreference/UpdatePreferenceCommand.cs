using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

public sealed record UpdatePreferenceCommand(
    IMessageContext MessageContext,
    Guid PreferenceOrUserId,
    UpdatePreferenceRequest? Preference) : ICommand;