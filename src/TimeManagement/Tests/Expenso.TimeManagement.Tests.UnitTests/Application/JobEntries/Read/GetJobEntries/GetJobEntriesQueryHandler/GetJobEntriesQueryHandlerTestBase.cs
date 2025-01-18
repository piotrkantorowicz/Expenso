using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Read.GetJobEntries.GetJobEntriesQueryHandler;

[TestFixture]
internal abstract class
    GetJobEntriesQueryHandlerTestBase : TestBase<
    Core.Application.JobEntries.Read.GetJobEntries.GetJobEntriesQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();

        _getJobEntriesQuery = new GetJobEntriesQuery(MessageContext: MessageContextFactoryMock.Object.Current(),
            Pagination: new Pagination(Page: PaginationDefaults.Page, Limit: PaginationDefaults.Limit),
            Sorters: new Sorting(Sorters: []), Payload: new GetJobEntriesRequest());

        _jobEntries =
        [
            new JobEntry
            {
                Id = Guid.NewGuid()
            },
            new JobEntry
            {
                Id = Guid.NewGuid()
            }
        ];

        TestCandidate =
            new Core.Application.JobEntries.Read.GetJobEntries.GetJobEntriesQueryHandler(
                jobEntryRepository: _jobEntryRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _jobEntryRepositoryMock.Reset();
        _jobEntryRepositoryMock = null!;
        _getJobEntriesQuery = null!;
        _jobEntries = null!;
        TestCandidate = null!;
    }

    protected GetJobEntriesQuery _getJobEntriesQuery = null!;
    protected List<JobEntry> _jobEntries = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
}