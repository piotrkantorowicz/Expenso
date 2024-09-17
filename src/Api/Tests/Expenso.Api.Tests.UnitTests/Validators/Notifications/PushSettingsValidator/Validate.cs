using Expenso.Communication.Proxy.DTO.Settings.Push;

namespace Expenso.Api.Tests.UnitTests.Validators.Notifications.PushSettingsValidator;

internal sealed class Validate : PushNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _pushNotificationSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _pushNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Push notification settings are required";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _pushNotificationSettings = new PushNotificationSettings(Enabled: null);

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _pushNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Push enabled flag must be provided";
        string error = validationResult[key: nameof(_pushNotificationSettings.Enabled)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_PushNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _pushNotificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}