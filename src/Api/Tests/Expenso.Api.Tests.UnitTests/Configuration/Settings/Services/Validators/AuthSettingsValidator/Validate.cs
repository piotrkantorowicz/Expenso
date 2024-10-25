using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.AuthSettingsValidator;

[TestFixture]
internal sealed class Validate : AuthSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_AuthServerIsInvalid()
    {
        // Arrange
        _authSettings = new AuthSettings
        {
            AuthServer = (AuthServer)999
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _authSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_authSettings.AuthServer),
            errorMessage: "AuthServer must be a valid value.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ValidSettingsProvided()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _authSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}