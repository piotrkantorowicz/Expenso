using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntryAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<JobEntry>> GetJobEntriesAsync(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}