using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.ApplicationSettingsValidator;

[TestFixture]
internal sealed class Validate : ApplicationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_InstanceIdIsNull()
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            InstanceId = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_applicationSettings.InstanceId),
            errorMessage: "Instance ID must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_InstanceIdIsEmpty()
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            InstanceId = Guid.Empty
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_applicationSettings.InstanceId),
            errorMessage: "Instance ID must be provided and cannot be empty.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_NameIsNullOrWhiteSpace(string name)
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            Name = name
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_applicationSettings.Name),
            errorMessage: "Name must be provided and cannot be empty.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_VersionIsNullOrWhiteSpace(string version)
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            Version = version
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_applicationSettings.Version),
            errorMessage: "Version must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_VersionMismatch()
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            Version = "2.0.0"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        string assemblyVersion = typeof(Program).Assembly.GetName().Version?.ToString()!;
        string[] assemblyVersionParts = assemblyVersion.Split(separator: '.');

        string expectedValidationMessage =
            $"Version mismatch. Expected: [{assemblyVersionParts[0]}.{assemblyVersionParts[1]}.{assemblyVersionParts[2]}], but got: [2.0.0].";

        validationResult.AssertSingleError(propertyName: nameof(_applicationSettings.Version),
            errorMessage: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ValidSettingsProvided()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}