using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Repositories;

internal sealed class PreferencesRepository : IPreferencesRepository
{
    private readonly IUserPreferencesDbContext _userPreferencesDbContext;

    public PreferencesRepository(IUserPreferencesDbContext userPreferencesDbContext)
    {
        _userPreferencesDbContext = userPreferencesDbContext ??
                                    throw new ArgumentNullException(paramName: nameof(userPreferencesDbContext));
    }

    public async Task<Preference?> GetAsync(PreferenceQuerySpecification preferenceQuerySpecification,
        CancellationToken cancellationToken)
    {
        return await _userPreferencesDbContext
            .Preferences.Tracking(useTracking: preferenceQuerySpecification.UseTracking)
            .IncludeMany(includeExpression: preferenceQuerySpecification.Include())
            .SingleOrDefaultAsync(predicate: preferenceQuerySpecification.Filter(),
                cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(PreferenceQuerySpecification preferenceQuerySpecification,
        CancellationToken cancellationToken)
    {
        return await _userPreferencesDbContext
            .Preferences.Tracking(useTracking: preferenceQuerySpecification.UseTracking)
            .IncludeMany(includeExpression: preferenceQuerySpecification.Include())
            .AnyAsync(predicate: preferenceQuerySpecification.Filter(), cancellationToken: cancellationToken);
    }

    public async Task<Preference> CreateAsync(Preference preference, CancellationToken cancellationToken)
    {
        await _userPreferencesDbContext.Preferences.AddAsync(entity: preference, cancellationToken: cancellationToken);
        await _userPreferencesDbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return preference;
    }

    public async Task<Preference> UpdateAsync(Preference preference, CancellationToken cancellationToken)
    {
        _userPreferencesDbContext.Preferences.Update(entity: preference);
        await _userPreferencesDbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return preference;
    }
}