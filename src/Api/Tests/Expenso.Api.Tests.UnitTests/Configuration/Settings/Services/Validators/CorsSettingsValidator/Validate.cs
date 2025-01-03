using Expenso.Api.Configuration.Settings.ApiSettings;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.CorsSettingsValidator;

[TestFixture]
internal sealed class Validate : CorsSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EnabledFlagIsNull()
    {
        // Arrange
        _corsSettings = _corsSettings with
        {
            Enabled = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _corsSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(CorsSettings.Enabled),
            errorMessage: "Cors enabled flag must be provided.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _corsSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(CorsSettings.AllowedOrigins),
            errorMessage: "AllowedOrigins cannot be null or empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _corsSettings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: $"{nameof(CorsSettings.AllowedOrigins)}[0]",
            errorMessage: "Origin cannot be empty and must be a valid URL or '*'.");
    }

    [Test]
    public void Should_NotReturnValidationErrors_When_CorsSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _corsSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}