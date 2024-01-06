using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Core.Repositories;

internal interface IPreferencesRepository
{
    Task<Preference?> GetByIdAsync(Guid preferenceId, bool useTracking, CancellationToken cancellationToken);

    Task<Preference?> GetByUserIdAsync(Guid userId, bool useTracking, CancellationToken cancellationToken);

    Task<Preference> CreateAsync(Preference preference, CancellationToken cancellationToken);

    Task<Preference> UpdateAsync(Preference preference, CancellationToken cancellationToken);
}