using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BackgroundJob(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));

    private TimeSpan _interval;
    private JobInstance? _jobInstance;
    private Guid _jobInstanceId;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IJobInstanceRepository jobInstanceRepository =
            scope.ServiceProvider.GetRequiredService<IJobInstanceRepository>() ??
            throw new ArgumentException(message: $"{nameof(jobInstanceRepository)} is null");

        _jobInstanceId = JobInstance.Default.Id;

        JobInstance? jobInstance =
            await jobInstanceRepository.GetAsync(id: _jobInstanceId, cancellationToken: cancellationToken);

        _jobInstance = jobInstance ??
                       throw new ArgumentException(message: $"Job entry type with id {_jobInstanceId} not found");

        _interval = TimeSpan.FromSeconds(value: _jobInstance?.RunningDelay ?? 10);
        await base.StartAsync(cancellationToken: cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(stoppingToken: stoppingToken);
            await Task.Delay(delay: _interval, cancellationToken: stoppingToken);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IJobExecution jobExecution = scope.ServiceProvider.GetRequiredService<IJobExecution>() ??
                                     throw new ArgumentException(message: $"{nameof(IJobExecution)} is null");

        await jobExecution.Execute(jobInstanceId: _jobInstanceId, interval: _interval, stoppingToken: stoppingToken);
    }
}