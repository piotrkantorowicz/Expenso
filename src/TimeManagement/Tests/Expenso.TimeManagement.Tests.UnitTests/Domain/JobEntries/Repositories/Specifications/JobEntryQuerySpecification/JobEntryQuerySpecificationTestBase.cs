using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Domain.JobEntries.Repositories.Specifications.
    JobEntryQuerySpecification;

[TestFixture]
internal abstract class JobEntryQuerySpecificationTestBase
{
    [SetUp]
    public void SetUp()
    {
        _jobEntries =
        [
            new JobEntry
            {
                Id = Guid.CreateVersion7(),
                JobInstanceId = Guid.CreateVersion7(),
                JobEntryStatusId = JobEntryStatus.Running.Id,
                CurrentRetries = 0,
                IsCompleted = false,
                RunAt = DateTime.UtcNow.AddMinutes(value: 30),
                LastRun = null,
                Triggers =
                [
                    new JobEntryTrigger
                    {
                        Id = Guid.CreateVersion7(),
                        EventData = "{}",
                        EventType = "IntegrationEvent"
                    }
                ]
            },
            new JobEntry
            {
                Id = Guid.CreateVersion7(),
                JobInstanceId = Guid.CreateVersion7(),
                JobEntryStatusId = JobEntryStatus.Completed.Id,
                CurrentRetries = 3,
                IsCompleted = true,
                RunAt = DateTime.UtcNow.AddSeconds(value: -30),
                LastRun = DateTime.UtcNow.AddSeconds(value: -30),
                Triggers =
                [
                    new JobEntryTrigger
                    {
                        Id = Guid.CreateVersion7(),
                        EventData = "{}",
                        EventType = "IntegrationEvent"
                    }
                ]
            },
            new JobEntry
            {
                Id = Guid.CreateVersion7(),
                JobInstanceId = Guid.CreateVersion7(),
                JobEntryStatusId = JobEntryStatus.Failed.Id,
                CurrentRetries = 3,
                IsCompleted = false,
                RunAt = DateTime.UtcNow.AddSeconds(value: -30),
                LastRun = DateTime.UtcNow.AddSeconds(value: -10)
            }
        ];

        _jobEntryQuerySpecification =
            new Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification();
    }

    protected List<JobEntry> _jobEntries = null!;

    protected Core.Domain.JobEntries.Repositories.Specifications.JobEntryQuerySpecification
        _jobEntryQuerySpecification = null!;
}