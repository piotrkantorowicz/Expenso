using Expenso.Shared.System.Time;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Persistence.EfCore;
using Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryStatusRepositoryTests;

[TestFixture]
internal abstract class JobEntryStatusRepositoryTestBase : TestBase<IJobEntryStatusRepository>
{
    [SetUp]
    public void Setup()
    {
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2024, month: 1, day: 1, hour: 12, minute: 0, second: 0,
                offset: TimeSpan.Zero));

        _jobEntryStatuses = new List<JobEntryStatus>
        {
            new()
            {
                Id = JobEntryStatusIds[index: 0],
                Name = "Pending",
                Description = "Job entry is pending"
            },
            new()
            {
                Id = JobEntryStatusIds[index: 1],
                Name = "IsRetrying",
                Description = "Job entry is retrying"
            },
            new()
            {
                Id = JobEntryStatusIds[index: 2],
                Name = "Completed",
                Description = "Job entry is completed"
            }
        };

        _jobEntryStatusesDbSetMock = _jobEntryStatuses.AsQueryable().BuildMockDbSet();
        _dbContextMock = new Mock<ITimeManagementDbContext>();
        _dbContextMock.Setup(expression: x => x.JobEntryStatuses).Returns(value: _jobEntryStatusesDbSetMock.Object);
        TestCandidate = new JobEntryStatusRepository(timeManagementDbContext: _dbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
        _jobEntryStatusesDbSetMock.Reset();
        _dbContextMock.Reset();
        _jobEntryStatuses.Clear();
    }

    protected static readonly IList<Guid> JobEntryStatusIds = new List<Guid>
    {
        new(g: "0194ba94-0f72-75b6-b0cb-872e03b45b58"),
        new(g: "0194ba94-0f72-7253-9313-fe4139cb402e"),
        new(g: "0194ba94-0f72-7df0-931b-eafed61839aa")
    };

    private Mock<IClock> _clockMock = null!;
    private Mock<ITimeManagementDbContext> _dbContextMock = null!;
    private Mock<DbSet<JobEntryStatus>> _jobEntryStatusesDbSetMock = null!;
    protected IList<JobEntryStatus> _jobEntryStatuses = null!;

    protected void ClearJobEntryStatuses()
    {
        _jobEntryStatuses.Clear();
    }
}