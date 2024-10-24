using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.FileSettingsValidator;

[TestFixture]
internal sealed class Validate : FileSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_StorageTypeIsInvalid()
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            StorageType = (FileStorageType)999
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.StorageType),
            errorMessage: "StorageType must be a valid value.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_RootPathIsInvalid()
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            RootPath = "invalidpath"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.RootPath),
            errorMessage: "RootPath must be a valid absolute path.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ImportDirectoryIsNullOrWhiteSpace(
        string importDirectory)
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            ImportDirectory = importDirectory
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.ImportDirectory),
            errorMessage: "ImportDirectory must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ImportDirectoryIsInvalidRelativePath()
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            ImportDirectory = "inva<lid:wpath?"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.ImportDirectory),
            errorMessage: "ImportDirectory must be a valid relative path.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ReportsDirectoryIsNullOrWhiteSpace(
        string reportsDirectory)
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            ReportsDirectory = reportsDirectory
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.ReportsDirectory),
            errorMessage: "ReportsDirectory must be provided and cannot be empty.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ReportsDirectoryIsInvalidRelativePath()
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            ReportsDirectory = "inva<lid:wpath?"
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_filesSettings.ReportsDirectory),
            errorMessage: "ReportsDirectory must be a valid relative path.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_FilesSettingsAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _filesSettings);

        // Assert
        validationResult.AssertNoErrors();
    }
}