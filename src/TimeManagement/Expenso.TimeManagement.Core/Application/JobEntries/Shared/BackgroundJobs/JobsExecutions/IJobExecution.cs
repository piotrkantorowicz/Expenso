namespace Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.JobsExecutions;

public interface IJobExecution
{
    Task Execute(Guid jobInstanceId, TimeSpan interval, CancellationToken stoppingToken);
}