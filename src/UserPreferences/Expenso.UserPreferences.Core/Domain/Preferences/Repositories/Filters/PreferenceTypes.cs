namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

[Flags]
internal enum PreferenceTypes
{
    None = 0,
    Finance = 1,
    Notification = 2,
    General = 4,
    All = Finance | Notification | General
}