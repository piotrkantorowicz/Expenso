using Expenso.Api.Configuration.Settings;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.CorsSettingsValidator;

[TestFixture]
internal sealed class Validate : CorsSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: "Cors settings are required.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledFlagIsNull()
    {
        // Arrange
        _corsSettings = _corsSettings with
        {
            Enabled = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _corsSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        string error = validationResult[key: nameof(CorsSettings.Enabled)];
        error.Should().Be(expected: "Cors enabled flag must be provided.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AllowedOriginsAreEmpty()
    {
        // Arrange
        _corsSettings = _corsSettings with
        {
            AllowedOrigins = []
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _corsSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        string error = validationResult[key: nameof(CorsSettings.AllowedOrigins)];
        error.Should().Be(expected: "AllowedOrigins cannot be null or empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AllowedOriginsContainsInvalidUrl()
    {
        // Arrange
        _corsSettings = _corsSettings with
        {
            AllowedOrigins = ["invalid-url"]
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _corsSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        string error = validationResult[key: nameof(CorsSettings.AllowedOrigins)];
        error.Should().Be(expected: "Origin cannot be empty and must be a valid URL.");
    }

    [Test]
    public void Should_NotReturnValidationErrors_When_CorsSettingsAreValid()
    {
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _corsSettings);

        // Assert
        validationResult.Should().BeEmpty();
    }
}