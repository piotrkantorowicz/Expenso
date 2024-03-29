using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryStatusRepository(ITimeManagementDbContext timeManagementDbContext)
    : IJobEntryStatusRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext =
        timeManagementDbContext ?? throw new ArgumentNullException(nameof(timeManagementDbContext));

    public async Task<JobEntryStatus?> GetAsync(string name, CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntryStatuses.Tracking(false)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobEntryStatus>> GetAsync(CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext.JobEntryStatuses.Tracking(false).ToListAsync(cancellationToken);
    }
}