namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.EfCoreValidator;

internal sealed class Validate : EfCoreSettingsValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        _efCoreSettings = null!;

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "EfCore settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ConnectionParametersIsNull()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            ConnectionParameters = null
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "ConnectionParameters must be provided and cannot be null.";
        string error = validationResult[key: nameof(_efCoreSettings.ConnectionParameters)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_MergeValidationResult_When_ConnectionParametersHasErrors()
    {
        // Arrange
        _efCoreSettings = _efCoreSettings with
        {
            ConnectionParameters = _efCoreSettings.ConnectionParameters! with
            {
                Host = string.Empty
            }
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Host must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_efCoreSettings.ConnectionParameters.Host)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "InMemory flag must be provided.";
        string error = validationResult[key: nameof(_efCoreSettings.InMemory)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "UseMigration flag must be provided.";
        string error = validationResult[key: nameof(_efCoreSettings.UseMigration)];
        error.Should().Be(expected: expectedValidationMessage);
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
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "UseSeeding flag must be provided.";
        string error = validationResult[key: nameof(_efCoreSettings.UseSeeding)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_EfCoreSettingsAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _efCoreSettings);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}