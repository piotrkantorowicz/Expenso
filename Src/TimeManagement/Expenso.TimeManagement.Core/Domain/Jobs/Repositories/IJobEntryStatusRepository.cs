using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

public interface IJobEntryStatusRepository
{
    Task<JobEntryStatus?> GetAsync(string name, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<JobEntryStatus>> GetAsync(CancellationToken cancellationToken);
}