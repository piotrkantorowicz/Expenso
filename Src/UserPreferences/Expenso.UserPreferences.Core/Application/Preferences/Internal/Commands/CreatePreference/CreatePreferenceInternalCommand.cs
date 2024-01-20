using Expenso.Shared.Commands;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;

public sealed record CreatePreferenceInternalCommand(CreatePreferenceInternalRequest Preference) : ICommand;