namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;

public interface IBudgetSharingRequestsExpirationJobExecution
{
    Task Execute(string jobTypeCode, CancellationToken stoppingToken);
}