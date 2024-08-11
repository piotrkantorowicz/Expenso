using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.JobExecutions;

internal abstract class JobExecutionTestBase : TestBase<JobExecution>
{
    protected Mock<IClock> _clockMock = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusRepositoryMock = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepositoryMock = null!;
    protected Mock<ILogger<JobExecution>> _loggerMock = null!;
    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Mock<ISerializer> _serializerMock = null!;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<JobExecution>>();
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusRepositoryMock = new Mock<IJobEntryStatusRepository>();
        _jobInstanceRepositoryMock = new Mock<IJobInstanceRepository>();

        _jobEntryStatusRepositoryMock
            .Setup(expression: x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: new List<JobEntryStatus>
            {
                JobEntryStatus.Running,
                JobEntryStatus.Retrying,
                JobEntryStatus.Cancelled,
                JobEntryStatus.Completed,
                JobEntryStatus.Failed
            });

        _serializerMock = new Mock<ISerializer>();
        _messageBrokerMock = new Mock<IMessageBroker>();
        _clockMock = new Mock<IClock>();

        TestCandidate = new JobExecution(logger: _loggerMock.Object, jobEntryRepository: _jobEntryRepositoryMock.Object,
            jobEntryStatusRepository: _jobEntryStatusRepositoryMock.Object, serializer: _serializerMock.Object,
            messageBroker: _messageBrokerMock.Object, clock: _clockMock.Object,
            jobInstanceRepository: _jobInstanceRepositoryMock.Object);
    }
}