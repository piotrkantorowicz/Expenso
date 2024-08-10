using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;

internal static class CreatePreferenceResponseMap
{
    public static CreatePreferenceResponse MapTo(Preference preference)
    {
        return new CreatePreferenceResponse(PreferenceId: preference.Id);
    }
}