using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BackgroundJob(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    private JobInstance? _jobInstance;
    private TimeSpan _interval;
    private Guid _jobInstanceId;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IJobInstanceRepository jobInstanceRepository =
            scope.ServiceProvider.GetRequiredService<IJobInstanceRepository>() ??
            throw new ArgumentException($"{nameof(jobInstanceRepository)} is null");

        _jobInstanceId = JobInstance.Default.Id;
        
        JobInstance? jobInstance = await jobInstanceRepository.GetAsync(_jobInstanceId, cancellationToken);
        
        _jobInstance = jobInstance ?? throw new ArgumentException($"Job entry type with id {_jobInstanceId} not found");
        _interval = TimeSpan.FromSeconds(_jobInstance?.RunningDelay ?? 10);
        
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IJobExecution jobExecution = scope.ServiceProvider.GetRequiredService<IJobExecution>() ??
                                     throw new ArgumentException($"{nameof(IJobExecution)} is null");

        await jobExecution.Execute(_jobInstanceId, _interval, stoppingToken);
    }
}