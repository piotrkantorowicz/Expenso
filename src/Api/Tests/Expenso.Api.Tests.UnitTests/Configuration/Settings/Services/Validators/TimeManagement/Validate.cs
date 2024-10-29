using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.TimeManagement;

[TestFixture]
internal sealed class Validate : TimeManagementSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_TimeManagementSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _timeManagementSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_AllowedEventsIsEmpty()
    {
        // Arrange
        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = []
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _timeManagementSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AllowedEventsIsNull()
    {
        // Arrange
        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _timeManagementSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_timeManagementSettings.AllowedEvents),
            errorMessage: "Allowed events must be provided.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AllowedEventsContainsInvalidValue()
    {
        // Arrange
        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = [AllowedEventType.BudgetPermissionRequestExpired, (AllowedEventType)int.MaxValue]
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _timeManagementSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: $"{nameof(_timeManagementSettings.AllowedEvents)}[1]",
            errorMessage: "Allowed events must be valid values.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AllowedEventsContainsTooManyValues()
    {
        // Arrange
        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = Enumerable.Repeat(AllowedEventType.BudgetPermissionRequestExpired, 101).ToArray()
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _timeManagementSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_timeManagementSettings.AllowedEvents),
            errorMessage: "Too many allowed events specified. Maximum is 100.");
    }
}