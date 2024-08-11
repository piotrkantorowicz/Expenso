using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal sealed class TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options)
    : DbContext(options: options), ITimeManagementDbContext
{
    public DbSet<JobEntry> JobEntries { get; set; } = null!;

    public DbSet<JobInstance> JobInstances { get; set; } = null!;

    public DbSet<JobEntryStatus> JobEntryStatuses { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity: entity).State;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await JobInstances.AddAsync(entity: JobInstance.Default, cancellationToken: cancellationToken);

        await JobEntryStatuses.AddRangeAsync(entities:
        [
            JobEntryStatus.Running, JobEntryStatus.Completed, JobEntryStatus.Failed, JobEntryStatus.Cancelled,
            JobEntryStatus.Retrying
        ], cancellationToken: cancellationToken);

        await SaveChangesAsync(cancellationToken: cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(schema: "TimeManagement");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}