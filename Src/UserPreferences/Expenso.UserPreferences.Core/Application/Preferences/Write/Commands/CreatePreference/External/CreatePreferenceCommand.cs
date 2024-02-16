using Expenso.Shared.Commands;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;

public sealed record CreatePreferenceCommand(CreatePreferenceRequest Preference) : ICommand;