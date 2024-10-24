using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO.Request;

using FluentValidation.Results;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandValidator;

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
        validationResult.AssertSingleError(propertyName: nameof(_cancelJobCommand.Payload),
            errorMessage: "The command payload must not be null.");
    }

    [Test, TestCase(arg: null), TestCase(arg: "00000000-0000-0000-0000-000000000000")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_JobEntryIdIsNullOrEmpty(string? jobEntryId)
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _cancelJobCommand with
        {
            Payload = new CancelJobEntryRequest(JobEntryId: jobEntryId is null ? null : new Guid(g: jobEntryId))
        });

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(_cancelJobCommand.Payload)}.{nameof(_cancelJobCommand.Payload.JobEntryId)}",
            errorMessage: "The job entry id must not be null or empty.");
    }
}