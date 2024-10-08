namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;

public sealed record GetPreferenceForCurrentUserRequest(
    GetPreferenceForCurrentUserRequest_PreferenceTypes? PreferenceType = null);