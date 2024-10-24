using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.PushSettingsValidator;

[TestFixture]
internal sealed class Validate : PushNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _pushNotificationSettings = new PushNotificationSettings(Enabled: null);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _pushNotificationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_pushNotificationSettings.Enabled),
            errorMessage: "Push enabled flag must be provided.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_PushNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _pushNotificationSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}