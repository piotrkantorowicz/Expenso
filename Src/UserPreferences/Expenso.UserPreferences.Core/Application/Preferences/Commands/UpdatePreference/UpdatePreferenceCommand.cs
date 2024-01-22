using Expenso.Shared.Commands;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;

public sealed record UpdatePreferenceCommand(Guid PreferenceOrUserId, UpdatePreferenceRequest? Preference) : ICommand;