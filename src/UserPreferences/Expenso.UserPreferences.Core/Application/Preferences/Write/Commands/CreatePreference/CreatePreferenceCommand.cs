using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

public sealed record CreatePreferenceCommand(IMessageContext MessageContext, CreatePreferenceRequest Preference)
    : ICommand;