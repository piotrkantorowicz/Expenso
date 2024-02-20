using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;

public sealed record CreatePreferenceCommand(IMessageContext MessageContext, CreatePreferenceRequest Preference)
    : ICommand;