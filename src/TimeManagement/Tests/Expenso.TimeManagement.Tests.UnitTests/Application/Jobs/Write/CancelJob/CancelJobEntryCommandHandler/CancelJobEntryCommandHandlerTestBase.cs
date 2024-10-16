using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Moq;

using TestCandidate = Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.CancelJobEntryCommandHandler;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandHandler;

[TestFixture]
internal abstract class CancelJobEntryCommandHandlerTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusReposiotry = new Mock<IJobEntryStatusRepository>();
        _jobEntryId = Guid.NewGuid();

        _cancelJobEntryCommand = new CancelJobEntryCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            CancelJobEntryRequest: new CancelJobEntryRequest(JobEntryId: _jobEntryId));

        _jobEntry = new JobEntry
        {
            Id = _jobEntryId.GetValueOrDefault(defaultValue: Guid.Empty)
        };

        TestCandidate = new TestCandidate(jobEntryRepository: _jobEntryRepositoryMock.Object,
            jobStatusRepository: _jobEntryStatusReposiotry.Object);
    }

    protected CancelJobEntryCommand _cancelJobEntryCommand = null!;
    protected JobEntry? _jobEntry;
    protected Guid? _jobEntryId;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusReposiotry = null!;
}