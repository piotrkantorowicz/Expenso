using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryStatusRepository : IJobEntryStatusRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext;

    public JobEntryStatusRepository(ITimeManagementDbContext timeManagementDbContext)
    {
        _timeManagementDbContext = timeManagementDbContext ??
                                   throw new ArgumentNullException(paramName: nameof(timeManagementDbContext));
    }

    public async Task<JobEntryStatus?> GetAsync(Guid id, CancellationToken cancellationToken, bool useTracking = false)
    {
        return await _timeManagementDbContext
            .JobEntryStatuses.Tracking(useTracking: useTracking)
            .SingleOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobEntryStatus>> GetManyAsync(CancellationToken cancellationToken,
        bool useTracking = false)
    {
        return await _timeManagementDbContext
            .JobEntryStatuses.Tracking(useTracking: useTracking)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}