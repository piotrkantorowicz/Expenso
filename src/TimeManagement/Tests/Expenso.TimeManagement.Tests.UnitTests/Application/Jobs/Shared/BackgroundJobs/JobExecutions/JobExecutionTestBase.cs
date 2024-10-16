﻿using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.JobExecutions;

[TestFixture]
internal abstract class JobExecutionTestBase : TestBase<JobExecution>
{
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILoggerService<JobExecution>>();
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusRepositoryMock = new Mock<IJobEntryStatusRepository>();
        _jobInstanceRepositoryMock = new Mock<IJobInstanceRepository>();

        _jobEntryStatusRepositoryMock
            .Setup(expression: x => x.GetAsync(It.IsAny<CancellationToken>(), true))
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

    protected Mock<IClock> _clockMock = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusRepositoryMock = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepositoryMock = null!;
    protected Mock<ILoggerService<JobExecution>> _loggerMock = null!;
    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Mock<ISerializer> _serializerMock = null!;
}