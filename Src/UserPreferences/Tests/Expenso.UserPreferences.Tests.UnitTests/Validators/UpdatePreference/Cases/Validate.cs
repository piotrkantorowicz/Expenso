using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators.UpdatePreference.Cases;

internal sealed class Validate : UpdatePreferenceCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CommandIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Command cannot be null.";
        string error = validationResult["command"];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            PreferenceOrUserId = userId
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Preferences or user id cannot be empty.";
        string error = validationResult[nameof(_updatePreferenceCommand.PreferenceOrUserId)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_FinancePreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = null
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Finance preferences cannot be null.";
        string error = validationResult[nameof(_updatePreferenceCommand.Preference.FinancePreference)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdateFinancePreferenceRequest(true, 5, true, -1)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of finance plan reviewers cannot be negative.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfFinancePlanReviewersIsGreaterThan10()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdateFinancePreferenceRequest(true, 5, true, 11)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of finance plan reviewers cannot be greater than 10.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdateFinancePreferenceRequest(true, -1, true, 5)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of sub finance plan sub owners cannot be negative.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_MaxNumberOfSubFinancePlanSubOwnersIsGreaterThan5()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                FinancePreference = new UpdateFinancePreferenceRequest(true, 6, true, 5)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Max number of sub finance plan sub owners cannot be greater than 5.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_NotificationPreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference = null!
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Notification preferences cannot be null.";
        string error = validationResult[nameof(_updatePreferenceCommand.Preference.NotificationPreference)];
        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsNegative()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference = new UpdateNotificationPreferenceRequest(true, -1)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Send finance report interval cannot be negative.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.NotificationPreference.SendFinanceReportInterval)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SendFinanceReportIntervalIsGreaterThan10()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                NotificationPreference = new UpdateNotificationPreferenceRequest(true, 32)
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Send finance report interval cannot be greater than 31.";

        string error = validationResult[
            nameof(_updatePreferenceCommand.Preference.NotificationPreference.SendFinanceReportInterval)];

        error.Should().Be(expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_GeneralPreferenceIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_updatePreferenceCommand with
        {
            Preference = _updatePreferenceCommand.Preference! with
            {
                GeneralPreference = null!
            }
        });

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "General preferences cannot be null.";
        string error = validationResult[nameof(_updatePreferenceCommand.Preference.GeneralPreference)];
        error.Should().Be(expectedValidationMessage);
    }
}