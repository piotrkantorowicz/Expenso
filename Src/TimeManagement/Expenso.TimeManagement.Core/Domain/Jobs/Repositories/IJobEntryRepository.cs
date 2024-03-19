using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntry(Guid id, CancellationToken cancellationToken);

    Task SaveAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}