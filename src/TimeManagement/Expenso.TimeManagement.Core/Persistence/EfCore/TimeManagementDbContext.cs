using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal sealed class TimeManagementDbContext : DbContext, ITimeManagementDbContext
{
    public TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options) : base(options: options)
    {
    }

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
        JobInstance? jobInstance =
            await JobInstances.FirstOrDefaultAsync(predicate: x => x.Id == JobInstance.Default.Id,
                cancellationToken: cancellationToken);

        if (jobInstance is null)
        {
            await JobInstances.AddAsync(entity: JobInstance.Default, cancellationToken: cancellationToken);
        }

        ICollection<JobEntryStatus> jobEntryStatuses =
        [
            JobEntryStatus.Running, JobEntryStatus.Completed, JobEntryStatus.Failed, JobEntryStatus.Cancelled,
            JobEntryStatus.Retrying
        ];

        foreach (JobEntryStatus jobEntryStatus in jobEntryStatuses)
        {
            JobEntryStatus? existingJobEntryStatus =
                await JobEntryStatuses.FirstOrDefaultAsync(predicate: x => x.Id == jobEntryStatus.Id,
                    cancellationToken: cancellationToken);

            if (existingJobEntryStatus is null)
            {
                await JobEntryStatuses.AddAsync(entity: jobEntryStatus, cancellationToken: cancellationToken);
            }
        }

        await SaveChangesAsync(cancellationToken: cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(schema: "TimeManagement");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}