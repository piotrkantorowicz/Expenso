using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal sealed class TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options)
    : DbContext(options), ITimeManagementDbContext
{
    public DbSet<JobEntry> JobEntries { get; set; } = null!;

    public DbSet<JobEntryType> JobEntryTypes { get; set; } = null!;

    public DbSet<JobEntryStatus> JobEntryStatuses { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity).State;
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TimeManagement");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}