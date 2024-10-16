namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.
    NotificationsSettingsValidator;

[TestFixture]
internal sealed class Validate : NotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _notificationSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Notification settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Enabled = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Enabled flag must be provided.";
        string error = validationResult[key: nameof(_notificationSettings.Enabled)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EnabledIsFalse()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Enabled = false
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EmailIsNull()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Email = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email notification settings must be provided.";
        string error = validationResult[key: nameof(_notificationSettings.Email)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_MergeValidationResult_When_EmailHasErrors()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Email = _notificationSettings.Email! with
            {
                From = string.Empty
            }
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email 'From' address must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_notificationSettings.Email.From)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_InAppIsNull()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            InApp = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "In-app notification settings must be provided.";
        string error = validationResult[key: nameof(_notificationSettings.InApp)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PushIsNull()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Push = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Push notification settings must be provided.";
        string error = validationResult[key: nameof(_notificationSettings.Push)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_NotificationSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _notificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}