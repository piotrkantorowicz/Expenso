using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Core.Persistence.EfCore.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Repositories;

internal sealed class PreferencesRepository(IUserPreferencesDbContext userPreferencesDbContext) : IPreferencesRepository
{
    private readonly IUserPreferencesDbContext _userPreferencesDbContext =
        userPreferencesDbContext ?? throw new ArgumentNullException(nameof(userPreferencesDbContext));

    public async Task<Preference?> GetAsync(PreferenceFilter preferenceFilter, CancellationToken cancellationToken)
    {
        return await _userPreferencesDbContext
            .Preferences.Tracking(preferenceFilter.UseTracking)
            .IncludeMany(preferenceFilter.ToIncludeExpressions())
            .SingleOrDefaultAsync(preferenceFilter.ToFilterExpression(), cancellationToken);
    }

    public async Task<bool> ExistsAsync(PreferenceFilter preferenceFilter, CancellationToken cancellationToken)
    {
        return await _userPreferencesDbContext
            .Preferences.Tracking(preferenceFilter.UseTracking)
            .IncludeMany(preferenceFilter.ToIncludeExpressions())
            .AnyAsync(preferenceFilter.ToFilterExpression(), cancellationToken);
    }

    public async Task<Preference> CreateAsync(Preference preference, CancellationToken cancellationToken)
    {
        await _userPreferencesDbContext.Preferences.AddAsync(preference, cancellationToken);
        await _userPreferencesDbContext.SaveChangesAsync(cancellationToken);

        return preference;
    }

    public async Task<Preference> UpdateAsync(Preference preference, CancellationToken cancellationToken)
    {
        _userPreferencesDbContext.Preferences.Update(preference);
        await _userPreferencesDbContext.SaveChangesAsync(cancellationToken);

        return preference;
    }
}