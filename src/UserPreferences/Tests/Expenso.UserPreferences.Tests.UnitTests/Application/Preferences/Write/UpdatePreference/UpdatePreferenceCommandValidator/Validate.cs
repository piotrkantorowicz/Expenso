using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
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
        validationResult.AssertSingleError(propertyName: "PreferenceId",
            errorMessage: "The preference ID must not be empty.");
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
        validationResult.AssertSingleError(propertyName: "Payload.FinancePreference",
            errorMessage: "The finance preference must not be empty.");
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
        validationResult.AssertSingleError(propertyName: "Payload.FinancePreference.MaxNumberOfFinancePlanReviewers",
            errorMessage: "The number of finance plan reviewers must be between 0 and 10.");
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
        validationResult.AssertSingleError(propertyName: "Payload.FinancePreference.MaxNumberOfFinancePlanReviewers",
            errorMessage: "The number of finance plan reviewers must be between 0 and 10.");
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
        validationResult.AssertSingleError(propertyName: "Payload.FinancePreference.MaxNumberOfSubFinancePlanSubOwners",
            errorMessage: "The number of finance plan sub-owners must be between 0 and 5.");
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
        validationResult.AssertSingleError(propertyName: "Payload.FinancePreference.MaxNumberOfSubFinancePlanSubOwners",
            errorMessage: "The number of finance plan sub-owners must be between 0 and 5.");
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
        validationResult.AssertSingleError(propertyName: "Payload.NotificationPreference",
            errorMessage: "The notification preference must not be empty.");
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
        validationResult.AssertSingleError(propertyName: "Payload.NotificationPreference.SendFinanceReportInterval",
            errorMessage: "The interval for sending the finance report must be between 0 and 31 days.");
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
        validationResult.AssertSingleError(propertyName: "Payload.NotificationPreference.SendFinanceReportInterval",
            errorMessage: "The interval for sending the finance report must be between 0 and 31 days.");
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
        validationResult.AssertSingleError(propertyName: "Payload.GeneralPreference",
            errorMessage: "The general preference must not be empty.");
    }
}