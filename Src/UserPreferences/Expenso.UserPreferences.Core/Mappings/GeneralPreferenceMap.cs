using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Mappings;

internal static class GeneralPreferenceMap
{
    public static GeneralPreferenceDto MapToDto(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceDto(generalPreference.UseDarkMode);
    }

    public static GeneralPreferenceContract MapToContract(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceContract(generalPreference.UseDarkMode);
    }

    public static GeneralPreference MapToModel(UpdateGeneralPreferenceDto updateGeneralPreferenceDto)
    {
        return GeneralPreference.Create(updateGeneralPreferenceDto.UseDarkMode);
    }
}