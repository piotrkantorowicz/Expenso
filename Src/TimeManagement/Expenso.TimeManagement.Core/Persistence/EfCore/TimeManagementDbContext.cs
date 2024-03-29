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

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await Database.MigrateAsync(cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await JobEntryTypes.AddAsync(JobEntryType.BudgetSharingRequestExpiration, cancellationToken);

        await JobEntryStatuses.AddRangeAsync([
            JobEntryStatus.Running, JobEntryStatus.Completed, JobEntryStatus.Failed, JobEntryStatus.Cancelled,
            JobEntryStatus.Retrying
        ], cancellationToken);

        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TimeManagement");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}