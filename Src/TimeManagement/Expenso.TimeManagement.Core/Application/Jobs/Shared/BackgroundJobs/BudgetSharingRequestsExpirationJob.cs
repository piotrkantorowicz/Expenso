using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BudgetSharingRequestsExpirationJob(IServiceProvider serviceProvider) : BackgroundService
{
    private const string JobTypeCode = "BS-REQ-EXP";
    
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    
    private JobEntryType? _jobEntryType;
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        
        IJobEntryTypeRepository jobEntryTypeRepository =
            scope.ServiceProvider.GetRequiredService<IJobEntryTypeRepository>() ??
            throw new ArgumentException($"{nameof(jobEntryTypeRepository)} is null");
        
        JobEntryType? jobEntryType = await jobEntryTypeRepository.GetAsync(JobTypeCode, cancellationToken);
        
        _jobEntryType = jobEntryType ??
                        throw new ArgumentException($"Job entry type with code {JobTypeCode} not found");
        
        await base.StartAsync(cancellationToken);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(_jobEntryType?.RunningDelay ?? 10), stoppingToken);
        }
    }
    
    private async Task DoWork(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        
        IBudgetSharingRequestsExpirationJobExecution budgetSharingRequestsExpirationJobExecution =
            scope.ServiceProvider.GetRequiredService<IBudgetSharingRequestsExpirationJobExecution>() ??
            throw new ArgumentException($"{nameof(IBudgetSharingRequestsExpirationJobExecution)} is null");
        
        await budgetSharingRequestsExpirationJobExecution.Execute(JobTypeCode, stoppingToken);
    }
}