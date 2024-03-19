using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobStatusRepository
{
    Task<JobEntryStatus?> GetAsync(string name, CancellationToken cancellationToken);
}