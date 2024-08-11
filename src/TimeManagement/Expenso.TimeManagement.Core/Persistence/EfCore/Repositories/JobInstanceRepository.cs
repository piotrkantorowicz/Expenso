using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobInstanceRepository(ITimeManagementDbContext timeManagementDbContext) : IJobInstanceRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext = timeManagementDbContext ??
                                                                         throw new ArgumentNullException(
                                                                             paramName: nameof(
                                                                                 timeManagementDbContext));

    public async Task<JobInstance?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobInstances.Tracking(useTracking: false)
            .FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }
}