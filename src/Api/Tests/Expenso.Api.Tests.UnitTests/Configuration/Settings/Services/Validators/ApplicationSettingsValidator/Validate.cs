using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

using NUnit.Framework;

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
        validationResult.AssertIsSingleError(propertyName: nameof(_applicationSettings.InstanceId),
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
        validationResult.AssertIsSingleError(propertyName: nameof(_applicationSettings.InstanceId),
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
        validationResult.AssertIsSingleError(propertyName: nameof(_applicationSettings.Name),
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
        validationResult.AssertContainsSingleError(propertyName: nameof(_applicationSettings.Version),
            errorMessage: "Version must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_VersionMismatch()
    {
        // Arrange
        Version providedVersion = Version.Parse(input: "2.0.0");

        _applicationSettings = _applicationSettings with
        {
            Version = providedVersion.ToString()
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _applicationSettings);

        // Assert
        Version assemblyVersion = typeof(Program).Assembly.GetName().Version!;

        string expectedValidationMessage =
            $"Version mismatch. Expected: [{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}], but got: [{providedVersion}].";

        validationResult.AssertIsSingleError(propertyName: nameof(_applicationSettings.Version),
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