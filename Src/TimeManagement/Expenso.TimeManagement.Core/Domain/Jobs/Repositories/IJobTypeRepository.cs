using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobTypeRepository
{
    Task<JobType?> GetAsync(string name, CancellationToken cancellationToken);
}