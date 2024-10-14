namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.EmailSettingsValidator;

internal sealed class Validate : EmailNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _emailNotificationSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email notification settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Enabled = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email enabled flag must be provided.";
        string error = validationResult[key: nameof(_emailNotificationSettings.Enabled)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsFalse()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Enabled = false
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_FromIsNullOrWhiteSpace(string? from)
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            From = from
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email 'From' address must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_emailNotificationSettings.From)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_FromIsInvalid()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            From = "invalid-email"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email 'From' address must be a valid email address.";
        string error = validationResult[key: nameof(_emailNotificationSettings.From)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ReplyToIsNullOrWhiteSpace(string? replyTo)
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            ReplyTo = replyTo
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email 'ReplyTo' address must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_emailNotificationSettings.ReplyTo)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ReplyToIsInvalid()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            ReplyTo = "invalid-email"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Email 'ReplyTo' address must be a valid email address.";
        string error = validationResult[key: nameof(_emailNotificationSettings.ReplyTo)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SmtpIsNull()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Smtp = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Smtp must be provided and cannot be null.";
        string error = validationResult[key: nameof(_emailNotificationSettings.Smtp)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_MergeValidationResult_When_SmtpHasErrors()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Smtp = _emailNotificationSettings.Smtp! with
            {
                Host = string.Empty
            }
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP host must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_emailNotificationSettings.Smtp.Host)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EmailNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _emailNotificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}