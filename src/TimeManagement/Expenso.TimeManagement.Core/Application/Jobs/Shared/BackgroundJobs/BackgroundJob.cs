using Expenso.Shared.System.Logging;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BackgroundJob : BackgroundService
{
    private readonly Guid _jobInstanceId;
    private readonly ILoggerService<BackgroundJob> _logger;
    private readonly IServiceProvider _serviceProvider;
    private TimeSpan _interval;
    private JobInstance? _jobInstance;

    public BackgroundJob(IServiceProvider serviceProvider, ILoggerService<BackgroundJob> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _jobInstanceId = JobInstance.Default.Id;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.BackgroundJobStarting,
            message: "Running the background job {JobInstanceId}", args: _jobInstanceId);

        using IServiceScope scope = _serviceProvider.CreateScope();

        IJobInstanceRepository jobInstanceRepository =
            scope.ServiceProvider.GetRequiredService<IJobInstanceRepository>() ??
            throw new ArgumentException(message: $"{nameof(jobInstanceRepository)} is null");

        JobInstance? jobInstance =
            await jobInstanceRepository.GetAsync(id: _jobInstanceId, cancellationToken: cancellationToken);

        _jobInstance = jobInstance ??
                       throw new ArgumentException(message: $"Job entry type with id {_jobInstanceId} not found");

        _interval = TimeSpan.FromSeconds(value: _jobInstance?.RunningDelay ?? 10);
        await base.StartAsync(cancellationToken: cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.BackgroundJobFinished,
            message: "Finished running the background job {JobInstanceId}", args: _jobInstanceId);

        return base.StopAsync(cancellationToken: cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug(eventId: LoggingUtils.BackgroundJobGeneralInformation,
            message: "Start processing entry for background job {JobInstanceId}...", args: _jobInstanceId);

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