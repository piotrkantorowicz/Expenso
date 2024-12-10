using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

internal interface IJobEntryStatusRepository
{
    Task<JobEntryStatus?> GetAsync(Guid id, CancellationToken cancellationToken, bool useTracking = true);

    Task<IReadOnlyCollection<JobEntryStatus>>
        GetManyAsync(CancellationToken cancellationToken, bool useTracking = true);
}