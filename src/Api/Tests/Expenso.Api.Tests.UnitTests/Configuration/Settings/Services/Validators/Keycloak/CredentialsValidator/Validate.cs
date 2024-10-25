using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Keycloak.CredentialsValidator;

[TestFixture]
internal sealed class Validate : KeycloakSettingsValidatorTestBase
{
    [Test, TestCase(arg: null), TestCase(arg: ""), TestCase(arg: "   ")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SecretIsNullOrWhiteSpace(string? secret)
    {
        // Arrange
        _credentials.Secret = secret!;

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _credentials);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_credentials.Secret),
            errorMessage: "Client secret must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RealmIsInvalid()
    {
        // Arrange
        _credentials.Secret = "1232";

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _credentials);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_credentials.Secret),
            errorMessage: "Client secret must be a valid GUID format.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_CredentialsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _credentials);

        // Assert
        validationResult.AssertNoErrors();
    }
}