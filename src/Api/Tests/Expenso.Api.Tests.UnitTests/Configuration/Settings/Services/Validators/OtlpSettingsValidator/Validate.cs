namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.OtlpSettingsValidator;

internal sealed class Validate : OtlpSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_OtlpSettingsAreNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Otlp settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _otlpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Service name must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_otlpSettings.ServiceName)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _otlpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "Service name can only contain alphanumeric characters and special characters.";

        string error = validationResult[key: nameof(_otlpSettings.ServiceName)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _otlpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Endpoint must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_otlpSettings.Endpoint)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _otlpSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Endpoint must be a valid URL.";
        string error = validationResult[key: nameof(_otlpSettings.Endpoint)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_OtlpSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _otlpSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}