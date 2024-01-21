using Expenso.Shared.Database.EfCore.NpSql.DbContexts;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal interface IUserPreferencesDbContext : IDbContext
{
    DbSet<Preference> Preferences { get; }
}