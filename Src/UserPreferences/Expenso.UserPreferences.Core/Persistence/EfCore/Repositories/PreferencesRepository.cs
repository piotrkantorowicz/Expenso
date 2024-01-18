using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Repositories;

internal sealed class PreferencesRepository(IUserPreferencesDbContext userPreferencesDbContext) : IPreferencesRepository
{
    private readonly IUserPreferencesDbContext _userPreferencesDbContext =
        userPreferencesDbContext ?? throw new ArgumentNullException(nameof(userPreferencesDbContext));

    public async Task<Preference?> GetByIdAsync(PreferenceId preferenceId, bool useTracking,
        CancellationToken cancellationToken)
    {
        IQueryable<Preference> preferencesQueryable = _userPreferencesDbContext.Preferences.AsQueryable();

        if (!useTracking)
        {
            preferencesQueryable = preferencesQueryable.AsNoTracking();
        }

        return await preferencesQueryable.SingleOrDefaultAsync(x => x.PreferenceId == preferenceId, cancellationToken);
    }

    public async Task<Preference?> GetByUserIdAsync(UserId userId, bool useTracking, CancellationToken cancellationToken)
    {
        IQueryable<Preference> userPreferencesQueryable = _userPreferencesDbContext.Preferences.AsQueryable();

        if (!useTracking)
        {
            userPreferencesQueryable = userPreferencesQueryable.AsNoTracking();
        }

        return await userPreferencesQueryable.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
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