using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandValidator;

internal sealed class Validate : UpdatePreferenceCommandValidatorTestBase
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
    public void Should_ReturnEmptyValidationResult_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            PreferenceOrUserId = userId
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Preferences or user id cannot be empty.";
        string error = validationResult[key: nameof(_updatePreferenceCommand.PreferenceOrUserId)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_FinancePreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = null
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Finance preferences cannot be null.";
        string error = validationResult[key: nameof(_updatePreferenceCommand.Preference.FinancePreference)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: -1)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of finance plan reviewers cannot be negative.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsGreaterThan10()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 11)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of finance plan reviewers cannot be greater than 10.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: -1, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of sub finance plan sub owners cannot be negative.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsGreaterThan5()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 6, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 5)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of sub finance plan sub owners cannot be greater than 5.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_NotificationPreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference = null!
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Notification preferences cannot be null.";
        string error = validationResult[key: nameof(_updatePreferenceCommand.Preference.NotificationPreference)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference =
                new UpdatePreferenceRequest_NotificationPreference(SendFinanceReportEnabled: true,
                    SendFinanceReportInterval: -1)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Send finance report interval cannot be negative.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.NotificationPreference.SendFinanceReportInterval)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsGreaterThan10()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference =
                new UpdatePreferenceRequest_NotificationPreference(SendFinanceReportEnabled: true,
                    SendFinanceReportInterval: 32)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Send finance report interval cannot be greater than 31.";

        string error = validationResult[
            key: nameof(_updatePreferenceCommand.Preference.NotificationPreference.SendFinanceReportInterval)];

        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_GeneralPreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command: _updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                GeneralPreference = null!
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "General preferences cannot be null.";
        string error = validationResult[key: nameof(_updatePreferenceCommand.Preference.GeneralPreference)];
        error.Should().Be(expected: expectedValidationMessage);
    }
}