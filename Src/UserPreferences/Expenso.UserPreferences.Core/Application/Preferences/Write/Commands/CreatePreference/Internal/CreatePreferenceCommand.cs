using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal;

public sealed record CreatePreferenceCommand(IMessageContext MessageContext, CreatePreferenceRequest Preference)
    : ICommand;