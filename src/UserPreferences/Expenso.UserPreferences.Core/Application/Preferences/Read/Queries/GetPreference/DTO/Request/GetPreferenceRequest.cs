namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;

public sealed record GetPreferenceRequest(
    Guid? PreferenceId = null,
    GetPreferenceRequestPreferenceIncludes? Includes = null);