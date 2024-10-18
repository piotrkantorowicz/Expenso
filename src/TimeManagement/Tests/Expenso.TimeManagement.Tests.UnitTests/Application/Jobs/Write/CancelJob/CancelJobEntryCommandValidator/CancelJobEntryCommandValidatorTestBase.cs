using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO.Request;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO.Request.Validators;

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
            Payload: new CancelJobEntryRequest(JobEntryId: _jobEntryId));

        TestCandidate = new TestCandidate(messageContextValidator: new MessageContextValidator(),
            cancelJobEntryRequestValidator: new CancelJobEntryRequestValidator());
    }

    protected CancelJobEntryCommand _cancelJobCommand = null!;
    private Guid? _jobEntryId;
}