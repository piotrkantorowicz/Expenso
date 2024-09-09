using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobInstanceRepository : IJobInstanceRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext;

    public JobInstanceRepository(ITimeManagementDbContext timeManagementDbContext)
    {
        _timeManagementDbContext = timeManagementDbContext ??
                                   throw new ArgumentNullException(paramName: nameof(timeManagementDbContext));
    }

    public async Task<JobInstance?> GetAsync(Guid id, CancellationToken cancellationToken, bool useTracking = true)
    {
        return await _timeManagementDbContext
            .JobInstances.Tracking(useTracking: useTracking)
            .FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }
}