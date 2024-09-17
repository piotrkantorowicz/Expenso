using Expenso.Shared.System.Configuration.Settings.Auth;

namespace Expenso.Api.Tests.UnitTests.Validators.AuthSettingsValidator;

internal sealed class Validate : AuthSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Auth settings are required";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerIsInvalid()
    {
        // Arrange
        _authSettings = new AuthSettings
        {
            AuthServer = (AuthServer)999
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _authSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "AuthServer must be a valid value";
        string error = validationResult[key: nameof(_authSettings.AuthServer)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ValidSettingsProvided()
    {
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _authSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}