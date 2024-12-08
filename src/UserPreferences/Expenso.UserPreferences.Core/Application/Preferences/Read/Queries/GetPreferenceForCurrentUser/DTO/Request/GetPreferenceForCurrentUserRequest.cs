namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;

public sealed record GetPreferenceForCurrentUserRequest(
    GetPreferenceForCurrentUserRequestPreferenceIncludes? Includes = null);