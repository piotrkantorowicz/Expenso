using Expenso.Shared.Database.EfCore.NpSql.DbContexts;
using Expenso.UserPreferences.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Data.Ef;

internal interface IUserPreferencesDbContext : IDbContext
{
    DbSet<Preference> Preferences { get; }
}