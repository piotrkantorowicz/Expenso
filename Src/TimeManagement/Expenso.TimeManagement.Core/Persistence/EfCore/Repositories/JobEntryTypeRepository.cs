using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

internal sealed class JobEntryTypeRepository(ITimeManagementDbContext timeManagementDbContext) : IJobEntryTypeRepository
{
    private readonly ITimeManagementDbContext _timeManagementDbContext =
        timeManagementDbContext ?? throw new ArgumentNullException(nameof(timeManagementDbContext));

    public async Task<JobEntryType?> GetAsync(string? code, CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntryTypes.Tracking(false)
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}