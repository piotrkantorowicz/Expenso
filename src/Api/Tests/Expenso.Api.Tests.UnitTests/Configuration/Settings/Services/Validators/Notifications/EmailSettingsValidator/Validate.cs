using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.EmailSettingsValidator;

[TestFixture]
internal sealed class Validate : EmailNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Enabled = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.Enabled),
            errorMessage: "Email enabled flag must be provided.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EnabledIsFalse()
    {
        // Arrange
        _emailNotificationSettings = _emailNotificationSettings with
        {
            Enabled = false
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertNoErrors();
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.From),
            errorMessage: "Email 'From' address must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.From),
            errorMessage: "Email 'From' address must be a valid email address.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.ReplyTo),
            errorMessage: "Email 'ReplyTo' address must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.ReplyTo),
            errorMessage: "Email 'ReplyTo' address must be a valid email address.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_emailNotificationSettings.Smtp),
            errorMessage: "Smtp must be provided and cannot be null.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(EmailNotificationSettings.Smtp)}.{nameof(EmailNotificationSettings.Smtp.Host)}",
            errorMessage: "SMTP host must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EmailNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _emailNotificationSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}