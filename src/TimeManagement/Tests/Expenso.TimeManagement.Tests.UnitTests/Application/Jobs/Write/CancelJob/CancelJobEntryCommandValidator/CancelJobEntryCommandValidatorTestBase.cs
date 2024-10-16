using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO;

using TestCandidate = Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.CancelJobEntryCommandValidator;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandValidator;

[TestFixture]
internal abstract class CancelJobEntryCommandValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryId = Guid.NewGuid();

        _cancelJobCommand = new CancelJobEntryCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            CancelJobEntryRequest: new CancelJobEntryRequest(JobEntryId: _jobEntryId));

        TestCandidate = new TestCandidate();
    }

    protected CancelJobEntryCommand _cancelJobCommand = null!;
    private Guid? _jobEntryId;
}