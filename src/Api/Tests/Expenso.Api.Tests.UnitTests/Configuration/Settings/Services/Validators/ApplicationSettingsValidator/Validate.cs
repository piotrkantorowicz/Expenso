namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.ApplicationSettingsValidator;

[TestFixture]
internal sealed class Validate : ApplicationSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Application settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_InstanceIdIsNull()
    {
        // Arrange
        _applicationSettings = _applicationSettings with
        {
            InstanceId = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Instance ID must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_applicationSettings.InstanceId)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Instance ID must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_applicationSettings.InstanceId)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Name must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_applicationSettings.Name)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Version must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_applicationSettings.Version)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        string assemblyVersion = typeof(Program).Assembly.GetName().Version?.ToString()!;
        string[] assemblyVersionParts = assemblyVersion.Split(separator: '.');

        string expectedValidationMessage =
            $"Version mismatch. Expected: [{assemblyVersionParts[0]}.{assemblyVersionParts[1]}.{assemblyVersionParts[2]}], but got: [2.0.0].";

        string error = validationResult[key: nameof(_applicationSettings.Version)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ValidSettingsProvided()
    {
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _applicationSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}