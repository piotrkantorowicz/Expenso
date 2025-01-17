using Expenso.Shared.Database.EfCore.Collections;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Pagination;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

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

    public async Task<JobEntry?> GetJobEntryAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntries.Tracking(useTracking: querySpecification.UseTracking)
            .IncludeMany(includeExpression: querySpecification.Include())
            .SingleOrDefaultAsync(predicate: querySpecification.Filter(), cancellationToken: cancellationToken);
    }

    public async Task<IPagedList<JobEntry>> GetJobEntriesAsync(JobEntryQuerySpecification querySpecification,
        Paging? pagination, Sorting? sorters, CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntries.Tracking(useTracking: querySpecification.UseTracking)
            .ApplySorting(sorting: sorters)
            .PaginationAsync(filter: querySpecification.Filter(), pagination: pagination,
                cancellationToken: cancellationToken);
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