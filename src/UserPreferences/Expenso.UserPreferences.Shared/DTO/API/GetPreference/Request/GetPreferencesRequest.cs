namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;

public sealed record GetPreferencesRequest(
    Guid? PreferenceId = null,
    Guid? UserId = null,
    GetPreferencesRequest_PreferenceTypes? PreferenceType = null);