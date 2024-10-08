using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

using FluentValidation.Results;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

internal sealed class Validate : UpdatePreferenceCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnNoErrors_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            PreferenceId = userId
        });

        // Assert
        AssertSingleError(validationResult: validationResult, propertyName: "PreferenceId",
            errorMessage: "Preference id cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_FinancePreferenceIsNull()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                FinancePreference = null
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult, propertyName: "Payload.FinancePreference",
            errorMessage: "Finance preference cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsNegative()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: -1)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.FinancePreference.MaxNumberOfFinancePlanReviewers",
            errorMessage: "Max number of finance plan reviewers must be between 0 and 10.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsGreaterThan10()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 11)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.FinancePreference.MaxNumberOfFinancePlanReviewers",
            errorMessage: "Max number of finance plan reviewers must be between 0 and 10.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsNegative()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: -1, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.FinancePreference.MaxNumberOfSubFinancePlanSubOwners",
            errorMessage: "Max number of sub finance plan sub owners must be between 0 and 5.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsGreaterThan5()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 6, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.FinancePreference.MaxNumberOfSubFinancePlanSubOwners",
            errorMessage: "Max number of sub finance plan sub owners must be between 0 and 5.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_NotificationPreferenceIsNull()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                NotificationPreference = null!
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult, propertyName: "Payload.NotificationPreference",
            errorMessage: "Notification preference cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsNegative()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                NotificationPreference =
                new UpdatePreferenceRequest_NotificationPreference(SendFinanceReportEnabled: true,
                    SendFinanceReportInterval: -1)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.NotificationPreference.SendFinanceReportInterval",
            errorMessage: "Send finance report interval must be between 0 and 31.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsGreaterThan10()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                NotificationPreference =
                new UpdatePreferenceRequest_NotificationPreference(SendFinanceReportEnabled: true,
                    SendFinanceReportInterval: 32)
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult,
            propertyName: "Payload.NotificationPreference.SendFinanceReportInterval",
            errorMessage: "Send finance report interval must be between 0 and 31.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_GeneralPreferenceIsNull()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _updatePreferenceCommand with
        {
            Payload = _updatePreferenceCommand.Payload! with
            {
                GeneralPreference = null!
            }
        });

        // Assert
        AssertSingleError(validationResult: validationResult, propertyName: "Payload.GeneralPreference",
            errorMessage: "General preference cannot be empty.");
    }
}