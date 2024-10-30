using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.EfCoreValidator;

[TestFixture]
internal sealed class Validate : EfCoreSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ConnectionParametersIsNull()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            ConnectionParameters = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _efCoreSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_efCoreSettings.ConnectionParameters),
            errorMessage: "ConnectionParameters must be provided and cannot be null.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_InMemoryIsNull()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            InMemory = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _efCoreSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_efCoreSettings.InMemory),
            errorMessage: "InMemory flag must be provided.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UseMigrationIsNull()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            UseMigration = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _efCoreSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_efCoreSettings.UseMigration),
            errorMessage: "UseMigration flag must be provided.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UseSeedingIsNull()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            UseSeeding = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _efCoreSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_efCoreSettings.UseSeeding),
            errorMessage: "UseSeeding flag must be provided.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EfCoreSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _efCoreSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}