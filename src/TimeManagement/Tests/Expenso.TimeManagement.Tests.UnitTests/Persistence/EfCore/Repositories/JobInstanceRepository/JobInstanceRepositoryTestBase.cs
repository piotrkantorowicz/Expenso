using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

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
        new(g: "19967114-32ef-4202-90c8-3aa590d14a03"),
        new(g: "87ddf365-e001-4949-abae-451d7ccd46c1"),
        new(g: "d3b1e36e-f188-4858-8d07-1b8bcd1b87fb")
    ];

    private Mock<IClock> _clockMock = null!;
    private Mock<ITimeManagementDbContext> _dbContextMock = null!;
    private Mock<DbSet<JobInstance>> _jobInstancesDbSetMock = null!;
    private IList<JobInstance> _jobInstances = null!;
}