using Expenso.Shared.Database.EfCore.Queryable;
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
            .FirstOrDefaultAsync(predicate: querySpecification.Filter(), cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobEntry>> GetJobEntriesAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken)
    {
        return await _timeManagementDbContext
            .JobEntries.Tracking(useTracking: querySpecification.UseTracking)
            .IncludeMany(includeExpression: querySpecification.Include())
            .Where(predicate: querySpecification.Filter())
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