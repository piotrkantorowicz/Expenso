namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;

[Flags]
public enum GetPreferencesRequest_PreferenceTypes
{
    None = 0,
    Finance = 1,
    Notification = 2,
    General = 4,
    All = Finance | Notification | General
}