using Expenso.Shared.Commands;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal;

public sealed record CreatePreferenceCommand(CreatePreferenceRequest Preference) : ICommand;