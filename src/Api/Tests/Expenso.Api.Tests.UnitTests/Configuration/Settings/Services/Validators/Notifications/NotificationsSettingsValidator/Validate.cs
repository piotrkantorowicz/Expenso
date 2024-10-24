using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.
    NotificationsSettingsValidator;

[TestFixture]
internal sealed class Validate : NotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _notificationSettings = _notificationSettings with
        {
            Enabled = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_notificationSettings.Enabled),
            errorMessage: "Enabled flag must be provided.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertNoErrors();
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_notificationSettings.Email),
            errorMessage: "Email notification settings must be provided.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(NotificationSettings.Email)}.{nameof(NotificationSettings.Email.From)}",
            errorMessage: "Email 'From' address must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(NotificationSettings.InApp),
            errorMessage: "In-app notification settings must be provided.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_notificationSettings.Push),
            errorMessage: "Push notification settings must be provided.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_NotificationSettingsAreValid()
    {
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _notificationSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}