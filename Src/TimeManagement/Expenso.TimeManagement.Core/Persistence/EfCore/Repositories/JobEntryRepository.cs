using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryRepository(ITimeManagementDbContext timeManagementDbContext) : IJobEntryRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext =
        timeManagementDbContext ?? throw new ArgumentNullException(nameof(timeManagementDbContext));
    
    public async Task<IReadOnlyCollection<JobEntry>> GetActiveJobEntries(CancellationToken cancellationToken,
        bool useTracking = false)
    {
        return await _timeManagementDbContext
            .JobEntries.Tracking(useTracking)
            .Where(x => x.Periods.Any(y =>
                            y.JobStatus == JobEntryStatus.Running || y.JobStatus != JobEntryStatus.Retrying) ||
                        x.Triggers.Count != 0)
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken)
    {
        if (_timeManagementDbContext.GetEntryState(jobEntry) == EntityState.Detached)
        {
            await _timeManagementDbContext.JobEntries.AddAsync(jobEntry, cancellationToken);
        }
        else
        {
            _timeManagementDbContext.JobEntries.Update(jobEntry);
        }
        
        await _timeManagementDbContext.SaveChangesAsync(cancellationToken);
    }
}