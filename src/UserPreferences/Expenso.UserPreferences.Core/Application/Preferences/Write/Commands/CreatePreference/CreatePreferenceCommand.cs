using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

public sealed record CreatePreferenceCommand(IMessageContext MessageContext, CreatePreferenceRequest? Payload)
    : ICommand;