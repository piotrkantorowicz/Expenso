using Expenso.UserPreferences.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Data.Ef;

internal sealed class UserPreferencesDbContext(DbContextOptions<UserPreferencesDbContext> options)
    : DbContext(options), IUserPreferencesDbContext
{
    public DbSet<Preference> Preferences { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("user_preferences");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}