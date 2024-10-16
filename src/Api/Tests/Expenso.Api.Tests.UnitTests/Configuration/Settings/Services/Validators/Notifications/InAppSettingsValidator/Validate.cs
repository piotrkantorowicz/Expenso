using Expenso.Communication.Shared.DTO.Settings.InApp;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.InAppSettingsValidator;

[TestFixture]
internal sealed class Validate : InAppNotificationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _inAppNotificationSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _inAppNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "In-app notification settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledIsNull()
    {
        // Arrange
        _inAppNotificationSettings = new InAppNotificationSettings(Enabled: null);

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _inAppNotificationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "In-app enabled flag must be provided.";
        string error = validationResult[key: nameof(_inAppNotificationSettings.Enabled)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_InAppNotificationSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _inAppNotificationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}