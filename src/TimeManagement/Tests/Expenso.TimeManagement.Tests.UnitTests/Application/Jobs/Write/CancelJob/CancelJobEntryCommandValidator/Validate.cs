using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandValidator;

[TestFixture]
internal sealed class Validate : CancelJobEntryCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CommandIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Command is required.";
        string error = validationResult[key: "Command"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _cancelJobCommand with
        {
            CancelJobEntryRequest = null
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Cancel job entry request is required.";
        string error = validationResult[key: nameof(_cancelJobCommand.CancelJobEntryRequest)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_JobEntryIdHasValue()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _cancelJobCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test, TestCase(arg: null), TestCase(arg: "00000000-0000-0000-0000-000000000000")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxRetriesIsNegative(string? jobEntryId)
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _cancelJobCommand with
        {
            CancelJobEntryRequest =
            new CancelJobEntryRequest(JobEntryId: jobEntryId is null ? null : new Guid(g: jobEntryId))
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "JobEntryId is required.";
        string error = validationResult[key: nameof(CancelJobEntryRequest.JobEntryId)];
        error.Should().Be(expected: expectedValidationMessage);
    }
}