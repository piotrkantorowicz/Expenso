using Expenso.Shared.Commands;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

public sealed record UpdatePreferenceCommand(Guid PreferenceOrUserId, UpdatePreferenceRequest? Preference) : ICommand;