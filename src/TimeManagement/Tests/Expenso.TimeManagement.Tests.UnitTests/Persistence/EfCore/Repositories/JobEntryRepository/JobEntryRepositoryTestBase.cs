using Expenso.Shared.System.Time;
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
        new(g: "0194ba94-0f72-72c3-bd93-0b5f64fbc171"),
        new(g: "0194ba94-0f72-757b-a8a5-9e4d754e1dd8"),
        new(g: "0194ba94-0f72-7ffa-b561-7193a7a1fbe8"),
        new(g: "0194ba94-0f72-70ee-bff1-c30649ee68ff"),
        new(g: "0194ba94-0f72-7df9-bdd1-40ed3c0b1594"),
        new(g: "0194ba94-0f72-7966-acef-68aab0124662")
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