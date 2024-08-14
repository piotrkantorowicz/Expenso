using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryRepository : IJobEntryRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext;

    public JobEntryRepository(ITimeManagementDbContext timeManagementDbContext)
    {
        _timeManagementDbContext = timeManagementDbContext ??
                                   throw new ArgumentNullException(paramName: nameof(timeManagementDbContext));
    }

    public async Task<IReadOnlyCollection<JobEntry>> GetActiveJobEntries(Guid jobInstanceId,
        CancellationToken cancellationToken, bool useTracking = false)
    {
        return await _timeManagementDbContext
            .JobEntries.Tracking(useTracking: useTracking)
            .Where(predicate: x => x.JobInstanceId == jobInstanceId &&
                                   (x.JobStatus == JobEntryStatus.Running || x.JobStatus == JobEntryStatus.Retrying) &&
                                   x.Triggers.Count > 0)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken)
    {
        if (_timeManagementDbContext.GetEntryState(entity: jobEntry) == EntityState.Detached)
        {
            await _timeManagementDbContext.JobEntries.AddAsync(entity: jobEntry, cancellationToken: cancellationToken);
        }
        else
        {
            _timeManagementDbContext.JobEntries.Update(entity: jobEntry);
        }

        await _timeManagementDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}