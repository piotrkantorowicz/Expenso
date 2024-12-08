namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;

[Flags]
public enum GetPreferenceRequestPreferenceIncludes
{
    None = 0,
    Finance = 1,
    Notification = 2,
    General = 4,
    All = Finance | Notification | General
}