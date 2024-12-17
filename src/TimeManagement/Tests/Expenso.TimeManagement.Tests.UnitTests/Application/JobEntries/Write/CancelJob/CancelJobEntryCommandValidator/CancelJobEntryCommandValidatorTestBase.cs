using Expenso.Shared.Commands.Validation.Validators;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request.Validators;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.CancelJob.CancelJobEntryCommandValidator;

[TestFixture]
internal abstract class
    CancelJobEntryCommandValidatorTestBase : TestBase<
    Core.Application.JobEntries.Write.CancelJobEntry.CancelJobEntryCommandValidator>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryId = Guid.NewGuid();

        _cancelJobCommand = new CancelJobEntryCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CancelJobEntryRequest(JobEntryId: _jobEntryId.Value));

        TestCandidate = new Core.Application.JobEntries.Write.CancelJobEntry.CancelJobEntryCommandValidator(
            messageContextValidator: new MessageContextValidator(),
            cancelJobEntryRequestValidator: new CancelJobEntryRequestValidator());
    }

    [TearDown]
    public void TearDown()
    {
        _cancelJobCommand = null!;
        _jobEntryId = null!;
        TestCandidate = null!;
    }

    protected CancelJobEntryCommand _cancelJobCommand = null!;
    private Guid? _jobEntryId;
}