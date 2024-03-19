using Microsoft.Extensions.Hosting;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BudgetSharingRequestsExpirationJob : BackgroundService
{
    public const string JobTypeName = "BudgetSharingRequestsExpirationJob";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}