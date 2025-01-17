using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Pagination;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntryAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<JobEntry>> GetJobEntriesAsync(JobEntryQuerySpecification querySpecification, Paging? pagination,
        Sorting? sorters, CancellationToken cancellationToken);

    Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}