using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

internal interface IJobInstanceRepository
{
    Task<JobInstance?> GetAsync(Guid id, CancellationToken cancellationToken, bool useTracking = true);
}