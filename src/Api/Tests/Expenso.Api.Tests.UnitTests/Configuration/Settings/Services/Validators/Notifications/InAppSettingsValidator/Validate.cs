using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.InAppSettingsValidator;

[TestFixture]
internal sealed class Validate : InAppNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _inAppNotificationSettings = new InAppNotificationSettings(Enabled: null);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _inAppNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_inAppNotificationSettings.Enabled),
            errorMessage: "In-app enabled flag must be provided.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_InAppNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _inAppNotificationSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}