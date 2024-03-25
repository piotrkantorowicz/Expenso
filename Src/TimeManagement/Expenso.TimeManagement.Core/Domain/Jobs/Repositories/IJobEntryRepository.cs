using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryRepository
{
    Task<IReadOnlyCollection<JobEntry>?> GetActiveJobEntries(CancellationToken cancellationToken,
        bool useTracking = false);

    Task SaveAsync(JobEntry jobEntry, CancellationToken cancellationToken);
}