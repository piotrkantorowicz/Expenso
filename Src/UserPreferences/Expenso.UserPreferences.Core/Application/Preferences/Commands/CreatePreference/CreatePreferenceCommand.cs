using Expenso.Shared.Commands;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;

public sealed record CreatePreferenceCommand(CreatePreferenceRequest Preference) : ICommand;