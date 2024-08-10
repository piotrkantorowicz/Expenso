using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobInstanceRepository
{
    Task<JobInstance?> GetAsync(Guid id, CancellationToken cancellationToken);
}