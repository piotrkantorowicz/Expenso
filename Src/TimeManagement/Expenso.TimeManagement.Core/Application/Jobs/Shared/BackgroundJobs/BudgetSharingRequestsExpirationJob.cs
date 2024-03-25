using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;

internal sealed class BudgetSharingRequestsExpirationJob(
    IServiceProvider serviceProvider,
    ISerializer serializer,
    IClock clock,
    IMessageBroker messageBroker) : BackgroundService
{
    private const string JobTypeCode = "BS-REQ-EXP";
    private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));

    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));

    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

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
            await Task.Delay(TimeSpan.FromSeconds(_jobEntryType?.Interval ?? 10), stoppingToken);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        ILogger<BudgetSharingRequestsExpirationJob>? logger = null;
        Guid? jobEntryId = null;

        try
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            logger = scope.ServiceProvider.GetRequiredService<ILogger<BudgetSharingRequestsExpirationJob>>() ??
                     throw new ArgumentException($"{nameof(logger)} is null");

            IJobEntryRepository jobEntryRepository = scope.ServiceProvider.GetRequiredService<IJobEntryRepository>() ??
                                                     throw new ArgumentException(
                                                         $"{nameof(jobEntryRepository)} is null");

            IReadOnlyCollection<JobEntry>? jobEntries = await jobEntryRepository.GetActiveJobEntries(stoppingToken);

            if (jobEntries is null || jobEntries.Count == 0)
            {
                logger.LogDebug($"No active {JobTypeCode} job entries found.");

                return;
            }

            IJobEntryStatusRepository jobEntryStatusRepository =
                scope.ServiceProvider.GetRequiredService<IJobEntryStatusRepository>() ??
                throw new ArgumentException($"{nameof(jobEntryStatusRepository)} is null");

            IReadOnlyCollection<JobEntryStatus> jobStatuses = await jobEntryStatusRepository.GetAsync(stoppingToken);

            foreach (JobEntry jobEntry in jobEntries)
            {
                jobEntryId = jobEntry.Id;

                if (jobEntry.Periods.Count == 0)
                {
                    logger.LogWarning("Skipping {JobTypeName} job entry with Id {JobEntryId} because it has no periods",
                        JobTypeCode, jobEntryId);

                    jobEntry.JobStatus = jobStatuses.First(x => x.Id == JobEntryStatus.Completed.Id);
                    await jobEntryRepository.SaveAsync(jobEntry, stoppingToken);

                    continue;
                }

                JobEntryPeriodIntervalHelper jobEntryPeriodIntervalHelper =
                    scope.ServiceProvider.GetRequiredService<JobEntryPeriodIntervalHelper>() ??
                    throw new ArgumentException($"{nameof(JobEntryPeriodIntervalHelper)} is null");

                foreach (JobEntryPeriod jobEntryPeriod in jobEntry.Periods)
                {
                    IntervalPrediction intervalPrediction =
                        jobEntryPeriodIntervalHelper.PredictInterval(jobEntryPeriod, _clock);

                    if (intervalPrediction.ShouldRun)
                    {
                        logger.LogDebug(
                            "Running {JobTypeName} job entry with Id {JobEntryId} and period {JobEntryPeriodTime}",
                            JobTypeCode, jobEntryId, jobEntryPeriod.RunAt.ToString("g"));

                        List<IIntegrationEvent?>? events = jobEntry
                            .Triggers?.Select(x =>
                            {
                                if (x.EventData is null || x.EventData.Length == 0 || x.EventType is null ||
                                    Type.GetType(x.EventType) is null)
                                {
                                    logger.LogWarning(
                                        "Skipping event trigger for {JobTypeName} job entry with Id {JobEntryId} and period {JobEntryPeriodTime} because it is invalid",
                                        JobTypeCode, jobEntryId, jobEntryPeriod.RunAt.ToString("g"));

                                    return null;
                                }

                                if (_serializer.Deserialize(x.EventData, Type.GetType(x.EventType)!) is
                                    IIntegrationEvent
                                    @event)
                                {
                                    return @event;
                                }

                                logger.LogWarning(
                                    "Failed to deserialize event trigger for {JobTypeName} job entry with Id {JobEntryId} and period {JobEntryPeriodTime}",
                                    JobTypeCode, jobEntryId, jobEntryPeriod.RunAt.ToString("g"));

                                return null;
                            })
                            .Where(x => x is not null)
                            .ToList();

                        if (events?.Count > 0)
                        {
                            foreach (IIntegrationEvent? integrationEvent in events)
                            {
                                await _messageBroker.PublishAsync(integrationEvent!, stoppingToken);
                            }

                            logger.LogDebug(
                                "Published {EventsCount} events for {JobTypeName} job entry with Id {JobEntryId} and period {JobEntryPeriodTime}",
                                events.Count, JobTypeCode, jobEntryId, jobEntryPeriod.RunAt.ToString("g"));

                            jobEntryPeriod.LastRun = intervalPrediction.LastRun;

                            if (jobEntryPeriod.Interval == JobEntryPeriodInterval.Once)
                            {
                                jobEntryPeriod.IsCompleted = true;
                            }
                        }
                    }

                    if (jobEntries.Any(x => x.Periods.All(y => y.IsCompleted == true)))
                    {
                        logger.LogDebug(
                            "All periods for {JobTypeName} job entry with Id {JobEntryId} have been completed",
                            JobTypeCode, jobEntryId);

                        jobEntry.JobStatus = jobStatuses.First(x => x.Id == JobEntryStatus.Completed.Id);
                    }

                    await jobEntryRepository.SaveAsync(jobEntry, stoppingToken);
                }
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while processing {JobTypeName} job entry with Id {JobEntryId}",
                JobTypeCode, jobEntryId);
        }
    }
}