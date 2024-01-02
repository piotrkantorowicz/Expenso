using Expenso.UserPreferences.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Data.Ef;

internal interface IUserPreferencesDbContext
{
    DbSet<Preference> Preferences { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}