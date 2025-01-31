using Expenso.Shared.System.Time;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobInstanceRepository;

[TestFixture]
internal abstract class JobInstanceRepositoryTestBase : TestBase<IJobInstanceRepository>
{
    [SetUp]
    public void Setup()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.Now);

        _jobInstances =
        [
            new JobInstance
            {
                Id = JobInstanceIds[index: 0],
                Name = "Instance1",
                RunningDelay = 5
            },
            new JobInstance
            {
                Id = JobInstanceIds[index: 1],
                Name = "Instance2",
                RunningDelay = 10
            },
            new JobInstance
            {
                Id = JobInstanceIds[index: 2],
                Name = "Instance3",
                RunningDelay = 15
            }
        ];

        _jobInstancesDbSetMock = _jobInstances.AsQueryable().BuildMockDbSet();
        _dbContextMock = new Mock<ITimeManagementDbContext>();
        _dbContextMock.Setup(expression: x => x.JobInstances).Returns(value: _jobInstancesDbSetMock.Object);

        TestCandidate =
            new Core.Persistence.EfCore.Repositories.JobInstanceRepository(
                timeManagementDbContext: _dbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
        _jobInstancesDbSetMock.Reset();
        _dbContextMock.Reset();
        _jobInstances.Clear();
    }

    protected static readonly IList<Guid> JobInstanceIds =
    [
        new(g: "0194ba94-0f72-73cf-8b7f-e8a4ee39bc7c"),
        new(g: "0194ba94-0f72-7e1d-b12a-6454ec9465d6"),
        new(g: "0194ba94-0f72-7dfc-aae8-c8c0d7678adf")
    ];

    private Mock<IClock> _clockMock = null!;
    private Mock<ITimeManagementDbContext> _dbContextMock = null!;
    private Mock<DbSet<JobInstance>> _jobInstancesDbSetMock = null!;
    private IList<JobInstance> _jobInstances = null!;
}