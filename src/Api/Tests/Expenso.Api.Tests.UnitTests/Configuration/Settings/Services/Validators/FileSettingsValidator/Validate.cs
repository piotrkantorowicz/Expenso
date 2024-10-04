using Expenso.Shared.System.Configuration.Settings.Files;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.FileSettingsValidator;

internal sealed class Validate : FileSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_filesSettingsAreNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "File settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_StorageTypeIsInvalid()
    {
        // Arrange
        _filesSettings = _filesSettings with
        {
            StorageType = (FileStorageType)999
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "StorageType must be a valid value.";
        string error = validationResult[key: nameof(_filesSettings.StorageType)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "RootPath must be a valid absolute path.";
        string error = validationResult[key: nameof(_filesSettings.RootPath)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "ImportDirectory must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_filesSettings.ImportDirectory)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "ImportDirectory must be a valid relative path.";
        string error = validationResult[key: nameof(_filesSettings.ImportDirectory)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "ReportsDirectory must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_filesSettings.ReportsDirectory)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "ReportsDirectory must be a valid relative path.";
        string error = validationResult[key: nameof(_filesSettings.ReportsDirectory)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_FilesSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _filesSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}