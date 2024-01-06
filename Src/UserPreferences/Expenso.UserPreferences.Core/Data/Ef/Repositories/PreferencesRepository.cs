using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Data.Ef.Repositories;

internal sealed class PreferencesRepository(IUserPreferencesDbContext userPreferencesDbContext) : IPreferencesRepository
{
    private readonly IUserPreferencesDbContext _userPreferencesDbContext =
        userPreferencesDbContext ?? throw new ArgumentNullException(nameof(userPreferencesDbContext));

    public async Task<Preference?> GetByIdAsync(Guid preferenceId, bool useTracking,
        CancellationToken cancellationToken)
    {
        IQueryable<Preference> preferencesQueryable = _userPreferencesDbContext.Preferences.AsQueryable();

        if (!useTracking)
        {
            preferencesQueryable = preferencesQueryable.AsNoTracking();
        }

        return await preferencesQueryable.SingleOrDefaultAsync(x => x.PreferencesId == preferenceId, cancellationToken);
    }

    public async Task<Preference?> GetByUserIdAsync(Guid userId, bool useTracking, CancellationToken cancellationToken)
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