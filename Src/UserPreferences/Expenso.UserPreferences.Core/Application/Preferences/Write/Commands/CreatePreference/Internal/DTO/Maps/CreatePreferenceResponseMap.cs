using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Maps;

internal static class CreatePreferenceResponseMap
{
    public static CreatePreferenceResponse MapTo(Preference preference)
    {
        return new CreatePreferenceResponse(preference.Id);
    }
}