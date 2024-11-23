namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;

public sealed record GetPreferenceForCurrentUserRequest(
    GetPreferenceForCurrentUserRequestPreferenceTypes? PreferenceType = null);