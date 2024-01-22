using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal sealed class UserPreferencesDbContext(DbContextOptions<UserPreferencesDbContext> options)
    : DbContext(options), IUserPreferencesDbContext
{
    public DbSet<Preference> Preferences { get; set; } = null!;

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("user_preferences");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}