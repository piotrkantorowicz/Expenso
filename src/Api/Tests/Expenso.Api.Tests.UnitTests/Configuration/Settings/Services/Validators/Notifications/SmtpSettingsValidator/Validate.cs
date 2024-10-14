namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator;

internal sealed class Validate : SmtpSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _smtpSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsNullOrWhiteSpace(string? host)
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Host = host!
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP host must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_smtpSettings.Host)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsInvalid()
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Host = "invalid_host"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP host must be a valid DNS name, IPv4, or IPv6 address.";
        string error = validationResult[key: nameof(_smtpSettings.Host)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PortIsInvalid()
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Port = -1
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP port must be a valid integer between 1 and 65535.";
        string error = validationResult[key: nameof(_smtpSettings.Port)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UsernameIsNullOrWhiteSpace(string username)
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Username = username
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP username must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_smtpSettings.Username)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UsernameIsInvalid()
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Username = "ab"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "SMTP username must be between 3 and 30 characters long and start with a letter.";

        string error = validationResult[key: nameof(_smtpSettings.Username)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PasswordIsNullOrWhiteSpace(string password)
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Password = password
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SMTP password must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_smtpSettings.Password)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PasswordIsInvalid()
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Password = "short"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "SMTP password must be between 8 and 20 characters long, with at least one uppercase letter, one lowercase letter, one digit, and one special character.";

        string error = validationResult[key: nameof(_smtpSettings.Password)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_SmtpSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _smtpSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}