using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntry(JobEntryQuerySpecification querySpecification, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<JobEntry>> GetJobEntries(JobEntryQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}