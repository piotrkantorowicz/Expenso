namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

[Flags]
internal enum PreferenceIncludes
{
    None = 0,
    Finance = 1,
    Notification = 2,
    General = 4,
    All = Finance | Notification | General
}