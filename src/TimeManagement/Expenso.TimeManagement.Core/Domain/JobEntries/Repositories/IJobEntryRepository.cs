using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Paging;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntryAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<JobEntry>> GetJobEntriesAsync(JobEntryQuerySpecification querySpecification,
        DatabasePagination? pagination, DatabaseSorting? sorters, CancellationToken cancellationToken);

    Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}