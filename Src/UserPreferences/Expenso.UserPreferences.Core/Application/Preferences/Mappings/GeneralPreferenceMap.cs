using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Mappings;

internal static class GeneralPreferenceMap
{
    public static GetGeneralPreferenceResponse MapToGetResponse(GeneralPreference generalPreference)
    {
        return new GetGeneralPreferenceResponse(generalPreference.UseDarkMode);
    }

    public static CreateGeneralPreferenceResponse MapToCreateResponse(GeneralPreference generalPreference)
    {
        return new CreateGeneralPreferenceResponse(generalPreference.UseDarkMode);
    }

    public static GeneralPreference MapToModel(UpdateGeneralPreferenceRequest updateGeneralPreferenceRequest)
    {
        return GeneralPreference.Create(updateGeneralPreferenceRequest.UseDarkMode);
    }

    public static GetGeneralPreferenceInternalResponse MapToInternalGetRequest(GeneralPreference generalPreference)
    {
        return new GetGeneralPreferenceInternalResponse(generalPreference.UseDarkMode);
    }

    public static CreateGeneralPreferenceInternalResponse MapToInternalCreateResponse(
        GeneralPreference generalPreference)
    {
        return new CreateGeneralPreferenceInternalResponse(generalPreference.UseDarkMode);
    }

    public static GeneralPreferenceInternalContract MapToInternalContract(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceInternalContract(generalPreference.UseDarkMode);
    }
}