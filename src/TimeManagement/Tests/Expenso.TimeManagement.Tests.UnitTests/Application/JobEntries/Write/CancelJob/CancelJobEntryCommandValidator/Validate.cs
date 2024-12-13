using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request;

using FluentValidation.Results;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.CancelJob.CancelJobEntryCommandValidator;

[TestFixture]
internal sealed class Validate : CancelJobEntryCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_JobEntryIdHasValue()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _cancelJobCommand);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _cancelJobCommand with
        {
            Payload = null
        });

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(_cancelJobCommand.Payload),
            errorMessage: "The command payload must not be null.");
    }

    [Test, TestCase(arg: "00000000-0000-0000-0000-000000000000")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_JobEntryIdIsNullOrEmpty(string? jobEntryId)
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _cancelJobCommand with
        {
            Payload = new CancelJobEntryRequest(JobEntryId: new Guid(g: jobEntryId!))
        });

        // Assert
        validationResult.AssertIsSingleError(
            propertyName: $"{nameof(_cancelJobCommand.Payload)}.{nameof(_cancelJobCommand.Payload.JobEntryId)}",
            errorMessage: "The job entry id must not be null or empty.");
    }
}