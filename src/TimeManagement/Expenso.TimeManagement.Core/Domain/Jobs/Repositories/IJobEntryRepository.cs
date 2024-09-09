using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryRepository
{
    Task<JobEntry?> GetJobEntry(Guid? jobEntryId, CancellationToken cancellationToken, bool useTracking = true);

    Task<IReadOnlyCollection<JobEntry>> GetActiveJobEntries(Guid? jobInstanceId, CancellationToken cancellationToken,
        bool useTracking = true);

    Task AddOrUpdateAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}