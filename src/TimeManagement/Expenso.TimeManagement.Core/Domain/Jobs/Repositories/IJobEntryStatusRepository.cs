using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryStatusRepository
{
    Task<JobEntryStatus?> GetAsync(Guid id, CancellationToken cancellationToken, bool useTracking = true);

    Task<IReadOnlyCollection<JobEntryStatus>> GetAsync(CancellationToken cancellationToken, bool useTracking = true);
}