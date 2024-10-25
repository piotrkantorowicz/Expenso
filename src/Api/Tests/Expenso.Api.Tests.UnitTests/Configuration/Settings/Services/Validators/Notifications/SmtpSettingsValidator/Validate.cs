using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator;

[TestFixture]
internal sealed class Validate : SmtpSettingsValidatorTestBase
{
    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsNullOrWhiteSpace(string? host)
    {
        // Arrange
        _smtpSettings = _smtpSettings with
        {
            Host = host
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Host),
            errorMessage: "SMTP host must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Host),
            errorMessage: "SMTP host must be a valid DNS name, IPv4, or IPv6 address.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Port),
            errorMessage: "SMTP port must be a valid integer between 1 and 65535.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Username),
            errorMessage: "SMTP username must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Username),
            errorMessage: "SMTP username must be between 3 and 30 characters long and start with a letter.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Password),
            errorMessage: "SMTP password must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_smtpSettings.Password),
            errorMessage:
            "SMTP password must be between 8 and 20 characters long, with at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_SmtpSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _smtpSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}