using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

[TestFixture]
internal sealed class Validate : OtlpSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_OtlpSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _otlpSettings);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ServiceNameIsNullOrWhiteSpace(string serviceName)
    {
        // Arrange
        _otlpSettings = _otlpSettings with
        {
            ServiceName = serviceName
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _otlpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_otlpSettings.ServiceName),
            errorMessage: "Service name must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ServiceNameIsInvalid()
    {
        // Arrange
        _otlpSettings = _otlpSettings with
        {
            ServiceName = "Invalid#Service"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _otlpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_otlpSettings.ServiceName),
            errorMessage: "Service name can only contain alphanumeric characters and special characters.");
    }

    [Test, TestCase(arg: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EndpointIsNullOrWhiteSpace(string? endpoint)
    {
        // Arrange
        _otlpSettings = _otlpSettings with
        {
            Endpoint = endpoint
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _otlpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_otlpSettings.Endpoint),
            errorMessage: "Endpoint must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_EndpointIsInvalidUrl()
    {
        // Arrange
        _otlpSettings = _otlpSettings with
        {
            Endpoint = "invalid-url"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _otlpSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_otlpSettings.Endpoint),
            errorMessage: "Endpoint must be a valid URL.");
    }
}