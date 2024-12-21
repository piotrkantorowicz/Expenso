using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal abstract class JobEntryRepositoryTestBase : TestBase<IJobEntryRepository>
{
    [SetUp]
    public void Setup()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.Now);

        _jobEntries =
        [
            new JobEntry
            {
                Id = _jobEntriesIds[index: 0],
                MaxRetries = 5,
                IsCompleted = false
            },
            new JobEntry
            {
                Id = _jobEntriesIds[index: 1],
                MaxRetries = 7,
                IsCompleted = false
            },
            new JobEntry
            {
                Id = _jobEntriesIds[index: 2],
                MaxRetries = 10,
                IsCompleted = true,
                LastRun = _clockMock.Object.UtcNow.AddHours(hours: -3)
            },
            new JobEntry
            {
                Id = _jobEntriesIds[index: 3],
                MaxRetries = 5,
                IsCompleted = true,
                LastRun = _clockMock.Object.UtcNow.AddHours(hours: -6)
            },
            new JobEntry
            {
                Id = _jobEntriesIds[index: 4],
                MaxRetries = 3,
                IsCompleted = true,
                LastRun = _clockMock.Object.UtcNow.AddHours(hours: -8)
            },
            new JobEntry
            {
                Id = _jobEntriesIds[index: 5],
                MaxRetries = 15,
                IsCompleted = false
            }
        ];

        _jobEntriesDbSetMock = _jobEntries.AsQueryable().BuildMockDbSet();
        _dbContextMock = new Mock<ITimeManagementDbContext>();
        _dbContextMock.Setup(expression: x => x.JobEntries).Returns(value: _jobEntriesDbSetMock.Object);

        TestCandidate =
            new Core.Persistence.EfCore.Repositories.JobEntryRepository(timeManagementDbContext: _dbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
        _jobEntriesDbSetMock.Reset();
        _dbContextMock.Reset();
        _jobEntries.Clear();
    }

    protected static IList<Guid> _jobEntriesIds =
    [
        new(g: "19967114-32ef-4202-90c8-3aa590d14a03"),
        new(g: "87ddf365-e001-4949-abae-451d7ccd46c1"),
        new(g: "d3b1e36e-f188-4858-8d07-1b8bcd1b87fb"),
        new(g: "9088d3fe-ac68-4f20-8925-ac8301563bf4"),
        new(g: "ec12f742-4c3b-4c40-b390-27ec12b31cf1"),
        new(g: "50796966-373d-4fb5-bb61-2b7499b0ce64")
    ];

    protected Mock<ITimeManagementDbContext> _dbContextMock = null!;
    protected Mock<DbSet<JobEntry>> _jobEntriesDbSetMock = null!;
    private IList<JobEntry> _jobEntries = null!;
    private Mock<IClock> _clockMock = null!;

    protected IList<JobEntry> JobEntries => _jobEntries.AsReadOnly();

    protected void AddJobEntry(JobEntry jobEntry)
    {
        _jobEntries.Add(item: jobEntry);
    }
}