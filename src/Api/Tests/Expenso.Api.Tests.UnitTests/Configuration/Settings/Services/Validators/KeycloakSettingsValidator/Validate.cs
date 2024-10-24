using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.KeycloakSettingsValidator;

[TestFixture]
internal sealed class Validate : KeycloakSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerUrlIsNull()
    {
        // Arrange
        _keycloakSettings.AuthServerUrl = null;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.AuthServerUrl),
            errorMessage: "Authorization server URL must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerUrlIsInvalid()
    {
        // Arrange
        _keycloakSettings.AuthServerUrl = "invalid-url";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.AuthServerUrl),
            errorMessage: "Authorization server URL must be a valid HTTP or HTTPS URL.");
    }

    [Test, TestCase(arg: null), TestCase(arg: ""), TestCase(arg: "   ")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RealmIsNullOrWhiteSpace(string? realm)
    {
        // Arrange
        _keycloakSettings.Realm = realm!;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.Realm),
            errorMessage: "Realm must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RealmIsInvalid()
    {
        // Arrange
        _keycloakSettings.Realm = "123";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.Realm),
            errorMessage: "Realm must be an alpha string with a length between 5 and 50 characters.");
    }

    [Test, TestCase(arg: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ResourceIsNullOrWhiteSpace(string? resource)
    {
        // Arrange
        _keycloakSettings.Resource = resource!;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.Resource),
            errorMessage: "Resource (client ID) must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ResourceIsInvalid()
    {
        // Arrange
        _keycloakSettings.Resource = "Invalid#Resource";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.Resource),
            errorMessage: "Resource (client ID) must be an alpha string with a length between 5 and 100 characters.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SslRequiredIsNullOrWhiteSpace(string? sslRequired)
    {
        // Arrange
        _keycloakSettings.SslRequired = sslRequired!;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.SslRequired),
            errorMessage: "SSL requirement must be specified and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SslRequiredIsInvalid()
    {
        // Arrange
        _keycloakSettings.SslRequired = "INVALID";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.SslRequired),
            errorMessage: "SSL requirement must be one of the predefined values.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_VerifyTokenAudienceIsNull()
    {
        // Arrange
        _keycloakSettings.VerifyTokenAudience = null;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.VerifyTokenAudience),
            errorMessage: "Token audience must be specified.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CredentialsAreNull()
    {
        // Arrange
        _keycloakSettings.Credentials = null!;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_keycloakSettings.Credentials),
            errorMessage: "Client secret must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_CredentialsSecretIsInvalid()
    {
        // Arrange
        _keycloakSettings.Credentials.Secret = "invalid-guid";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(_keycloakSettings.Credentials)}.{nameof(_keycloakSettings.Credentials.Secret)}",
            errorMessage: "Client secret must be a valid GUID format.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_KeycloakSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _keycloakSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}