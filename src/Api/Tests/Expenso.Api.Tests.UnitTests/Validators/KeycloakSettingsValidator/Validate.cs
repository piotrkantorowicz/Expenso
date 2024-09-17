namespace Expenso.Api.Tests.UnitTests.Validators.KeycloakSettingsValidator;

internal sealed class Validate : KeycloakSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_KeycloakSettingsAreNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Keycloak settings are required";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arguments: null)]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerUrlIsNullOrEmpty(string? authServerUrl)
    {
        // Arrange
        _keycloakSettings.AuthServerUrl = authServerUrl;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Authorization server URL must be provided and cannot be empty";
        string error = validationResult[key: nameof(_keycloakSettings.AuthServerUrl)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerUrlIsInvalid()
    {
        // Arrange
        _keycloakSettings.AuthServerUrl = "invalid-url";

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Authorization server URL must be a valid HTTP or HTTPS URL";
        string error = validationResult[key: nameof(_keycloakSettings.AuthServerUrl)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arg: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RealmIsNullOrEmpty(string? realm)
    {
        // Arrange
        _keycloakSettings.Realm = realm!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Realm must be provided and cannot be empty";
        string error = validationResult[key: nameof(_keycloakSettings.Realm)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RealmIsInvalid()
    {
        // Arrange
        _keycloakSettings.Realm = "123";

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "Realm must be an alpha string with a length between 5 and 50 characters";

        string error = validationResult[key: nameof(_keycloakSettings.Realm)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arg: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ResourceIsNullOrEmpty(string? resource)
    {
        // Arrange
        _keycloakSettings.Resource = resource!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Resource (client Id) must be provided and cannot be empty";
        string error = validationResult[key: nameof(_keycloakSettings.Resource)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ResourceIsInvalid()
    {
        // Arrange
        _keycloakSettings.Resource = "Invalid#Resource";

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "Resource (client Id) must be an alpha string with a length between 5 and 100 characters";

        string error = validationResult[key: nameof(_keycloakSettings.Resource)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SslRequiredIsNullOrEmpty(string? sslRequired)
    {
        // Arrange
        _keycloakSettings.SslRequired = sslRequired!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SSL requirement must be specified and cannot be empty";
        string error = validationResult[key: nameof(_keycloakSettings.SslRequired)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SslRequiredIsInvalid()
    {
        // Arrange
        _keycloakSettings.SslRequired = "INVALID";

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "SSL requirement must be one of the predefined values";
        string error = validationResult[key: nameof(_keycloakSettings.SslRequired)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_VerifyTokenAudienceIsNull()
    {
        // Arrange
        _keycloakSettings.VerifyTokenAudience = null;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "VerifyTokenAudience must be specified";
        string error = validationResult[key: nameof(_keycloakSettings.VerifyTokenAudience)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CredentialsAreNull()
    {
        // Arrange
        _keycloakSettings.Credentials = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Client secret must be provided and cannot be empty";
        string error = validationResult[key: nameof(_keycloakSettings.Credentials)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CredentialsSecretIsInvalid()
    {
        // Arrange
        _keycloakSettings.Credentials.Secret = "invalid-guid";

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Client secret must be a valid GUID format";
        string error = validationResult[key: nameof(_keycloakSettings.Credentials.Secret)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_KeycloakSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _keycloakSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}