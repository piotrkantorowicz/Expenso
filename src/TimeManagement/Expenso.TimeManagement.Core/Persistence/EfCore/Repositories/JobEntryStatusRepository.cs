using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryStatusRepository(ITimeManagementDbContext timeManagementDbContext)
    : IJobEntryStatusRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext = timeManagementDbContext ??
                                                                         throw new ArgumentNullException(
                                                                             paramName: nameof(
                                                                                 timeManagementDbContext));

    public async Task<JobEntryStatus?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntryStatuses.Tracking(useTracking: false)
            .FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobEntryStatus>> GetAsync(CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntryStatuses.Tracking(useTracking: false)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}