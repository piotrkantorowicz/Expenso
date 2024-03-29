using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.Migrations;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal interface IUserPreferencesDbContext : IDbContext, IDoNotSeed
{
    DbSet<Preference> Preferences { get; }
}