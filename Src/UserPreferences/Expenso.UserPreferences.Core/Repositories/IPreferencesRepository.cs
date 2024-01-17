using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Core.Repositories;

internal interface IPreferencesRepository
{
    Task<Preference?> GetByIdAsync(PreferenceId preferenceId, bool useTracking, CancellationToken cancellationToken);

    Task<Preference?> GetByUserIdAsync(UserId userId, bool useTracking, CancellationToken cancellationToken);

    Task<Preference> CreateAsync(Preference preference, CancellationToken cancellationToken);

    Task<Preference> UpdateAsync(Preference preference, CancellationToken cancellationToken);
}