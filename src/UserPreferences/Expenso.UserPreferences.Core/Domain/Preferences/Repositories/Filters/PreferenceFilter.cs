namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

internal sealed record PreferenceFilter(
    Guid? Id = null,
    Guid? UserId = null,
    bool? UseTracking = null,
    bool? IncludeFinancePreferences = null,
    bool? IncludeNotificationPreferences = null,
    bool? IncludeGeneralPreferences = null);