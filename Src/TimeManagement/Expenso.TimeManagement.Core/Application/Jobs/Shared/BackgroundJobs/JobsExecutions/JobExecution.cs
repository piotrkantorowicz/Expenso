using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.Logging;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;

internal sealed class JobExecution : IJobExecution
{
    private readonly IClock _clock;
    private readonly IJobEntryRepository _jobEntryRepository;
    private readonly IJobEntryStatusRepository _jobEntryStatusRepository;
    private readonly IJobInstanceRepository _jobInstanceRepository;
    private readonly ILogger<JobExecution> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly ISerializer _serializer;

    public JobExecution(ILogger<JobExecution> logger, IJobEntryRepository jobEntryRepository,
        IJobEntryStatusRepository jobEntryStatusRepository, ISerializer serializer, IMessageBroker messageBroker,
        IClock clock, IJobInstanceRepository jobInstanceRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));

        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));

        _jobEntryStatusRepository = jobEntryStatusRepository ??
                                    throw new ArgumentNullException(paramName: nameof(jobEntryStatusRepository));

        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
        _messageBroker = messageBroker ?? throw new ArgumentNullException(paramName: nameof(messageBroker));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));

        _jobInstanceRepository = jobInstanceRepository ??
                                 throw new ArgumentNullException(paramName: nameof(jobInstanceRepository));
    }

    public async Task Execute(Guid jobInstanceId, TimeSpan interval, CancellationToken stoppingToken)
    {
        try
        {
            DateTime jobExecutionStartTime = _clock.UtcNow.DateTime;

            JobInstance? jobInstance =
                await _jobInstanceRepository.GetAsync(id: jobInstanceId, cancellationToken: stoppingToken);

            if (jobInstance is null)
            {
                _logger.LogInformation(message: "Job instance with id: {JobInstanceId} hasn't been found",
                    jobInstanceId);

                return;
            }

            IReadOnlyCollection<JobEntry> jobEntries =
                await _jobEntryRepository.GetActiveJobEntries(jobInstanceId: jobInstance.Id,
                    cancellationToken: stoppingToken);

            if (jobEntries.Count == 0)
            {
                _logger.LogInformation(message: "No active job entries found. JobInstanceId: {JobInstanceId}",
                    jobInstanceId);

                return;
            }

            IReadOnlyCollection<JobEntryStatus> jobStatuses =
                await _jobEntryStatusRepository.GetAsync(cancellationToken: stoppingToken);

            if (jobStatuses.Count == 0)
            {
                _logger.LogWarning(message: "No job entry statuses found");

                return;
            }

            foreach (JobEntry jobEntry in jobEntries)
            {
                if (jobEntry.Triggers.Count == 0)
                {
                    _logger.LogWarning(
                        message:
                        "Skipping job entry with id {JobEntryId} because it has no triggers. JobInstanceId: {JobInstanceId}",
                        jobEntry.Id, jobInstanceId);

                    continue;
                }

                try
                {
                    bool shouldRun;
                    DateTime? occurence = null;

                    if (jobEntry.CronExpression is null)
                    {
                        shouldRun = jobEntry.LastRun is null;

                        if (shouldRun is false)
                        {
                            _logger.LogWarning(
                                message:
                                "Skipping job entry with id {JobEntryId} because it ended. JobInstanceId: {JobInstanceId}",
                                jobEntry.Id, jobInstanceId);

                            SetJobAsCompleted(currentJobEntry: jobEntry, jobTypeInstance: jobInstanceId);

                            continue;
                        }
                    }
                    else
                    {
                        CrontabSchedule? schedule = CrontabSchedule.Parse(expression: jobEntry.CronExpression);
                        occurence = schedule?.GetNextOccurrence(baseTime: jobExecutionStartTime);

                        if (occurence is null)
                        {
                            _logger.LogWarning(
                                message:
                                "Skipping job entry with Id {JobEntryId} because the job is ended. JobInstanceId: {JobInstanceId}",
                                jobEntry.Id, jobInstanceId);

                            SetJobAsCompleted(currentJobEntry: jobEntry, jobTypeInstance: jobInstanceId);

                            continue;
                        }

                        shouldRun = occurence >= jobExecutionStartTime && occurence <= jobExecutionStartTime + interval;

                        if (shouldRun is false)
                        {
                            _logger.LogDebug(
                                message:
                                "Skipping job entry with Id {JobEntryId} because it's out of the actual run. JobInstanceId: {JobInstanceId}",
                                jobEntry.Id, jobInstanceId);

                            continue;
                        }
                    }

                    _logger.LogDebug(message: "Running job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                        jobEntry.Id, jobInstanceId);

                    List<IIntegrationEvent?> events = jobEntry
                        .Triggers.Select(selector: x =>
                        {
                            if (x.EventData is null || x.EventData.Length == 0 || x.EventType is null ||
                                Type.GetType(typeName: x.EventType) is null)
                            {
                                _logger.LogWarning(
                                    message:
                                    "Skipping triggering events for job entry with Id {JobEntryId} because it's invalid. JobInstanceId: {JobInstanceId}",
                                    jobEntry.Id, jobInstanceId);

                                return null;
                            }

                            object? @event = _serializer.Deserialize(value: x.EventData,
                                type: Type.GetType(typeName: x.EventType)!);

                            if (@event is not null)
                            {
                                return (IIntegrationEvent)@event;
                            }

                            _logger.LogWarning(
                                message:
                                "Failed to deserialize event trigger for job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                                jobEntry.Id, jobInstanceId);

                            return null;
                        })
                        .Where(predicate: x => x is not null)
                        .ToList();

                    if (events.Count > 0)
                    {
                        foreach (IIntegrationEvent? integrationEvent in events)
                        {
                            await _messageBroker.PublishAsync(@event: integrationEvent!,
                                cancellationToken: stoppingToken);
                        }

                        _logger.LogDebug(
                            message:
                            "Published events: {Events} for job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                            string.Join(separator: ",", values: events.Select(selector: x => x!.GetType().FullName)),
                            jobEntry.Id, jobInstanceId);
                    }

                    jobEntry.LastRun = occurence ?? jobExecutionStartTime;
                }
                catch (Exception ex)
                {
                    _logger.LogError(exception: ex,
                        message:
                        "An error occurred while processing and job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                        jobEntry.Id, jobInstanceId);

                    jobEntry.JobStatus =
                        jobEntry.MaxRetries is not null && jobEntry.MaxRetries >= (jobEntry.CurrentRetries ?? 0)
                            ? jobStatuses.First(predicate: x => x.IsRetrying())
                            : jobStatuses.First(predicate: x => x.IsFailed());

                    if (jobEntry.JobStatus.IsRetrying())
                    {
                        jobEntry.CurrentRetries++;
                    }
                }
                finally
                {
                    await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: stoppingToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(exception: ex,
                message: "An error occurred while processing job. JobInstanceId: {JobInstanceId}", jobInstanceId);
        }
    }

    private void SetJobAsCompleted(JobEntry currentJobEntry, Guid jobTypeInstance)
    {
        _logger.LogDebug(
            message:
            "All periods for job entry with Id {JobEntryId} have been completed. JobInstanceId: {JobInstanceId}",
            currentJobEntry.Id, jobTypeInstance);

        currentJobEntry.JobStatus = JobEntryStatus.Completed;
        currentJobEntry.IsCompleted = true;
    }
}