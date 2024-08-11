using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.UserPreferences.Core.Persistence.EfCore;

internal sealed class UserPreferencesDbContext(DbContextOptions<UserPreferencesDbContext> options)
    : DbContext(options: options), IUserPreferencesDbContext
{
    public DbSet<Preference> Preferences { get; set; } = null!;

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(schema: "UserPreferences");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}