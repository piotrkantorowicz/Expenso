using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

internal interface IPreferencesRepository
{
    Task<Preference?> GetAsync(PreferenceQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(PreferenceQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<Preference> CreateAsync(Preference preference, CancellationToken cancellationToken);

    Task<Preference> UpdateAsync(Preference preference, CancellationToken cancellationToken);
}