using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntry;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Read.GetJobEntry.GetJobEntryQueryHandler;

[TestFixture]
internal abstract class
    GetJobEntryQueryHandlerTestBase : TestBase<Core.Application.JobEntries.Read.GetJobEntry.GetJobEntryQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryId = Guid.CreateVersion7();

        _getJobEntryQuery = new GetJobEntryQuery(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetJobEntryRequest(JobEntryId: _jobEntryId));

        _jobEntry = new JobEntry
        {
            Id = _jobEntryId
        };

        TestCandidate =
            new Core.Application.JobEntries.Read.GetJobEntry.GetJobEntryQueryHandler(
                jobEntryRepository: _jobEntryRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _jobEntryRepositoryMock.Reset();
        _jobEntryRepositoryMock = null!;
        _getJobEntryQuery = null!;
        _jobEntry = null!;
        TestCandidate = null!;
    }

    protected GetJobEntryQuery _getJobEntryQuery = null!;
    protected JobEntry? _jobEntry;
    protected Guid _jobEntryId;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
}