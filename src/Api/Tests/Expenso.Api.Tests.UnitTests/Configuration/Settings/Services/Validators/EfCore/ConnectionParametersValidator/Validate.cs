namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator;

internal sealed class Validate : ConnectionParametersValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_SettingsIsNull()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: null!);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Connection parameters settings are required.";
        string error = validationResult[key: "Settings"];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsNullOrEmpty()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Host = string.Empty
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Host must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.Host)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Host = "invalid_host"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Host must be a valid DNS name or IP address.";
        string error = validationResult[key: nameof(_connectionParameters.Host)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PortIsNullOrEmpty()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Port = string.Empty
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Port must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.Port)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PortIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Port = "70000"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Port must be a valid integer between 1 and 65535.";
        string error = validationResult[key: nameof(_connectionParameters.Port)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DefaultDatabaseIsNullOrEmpty(
        string? defaultDatabase)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            DefaultDatabase = defaultDatabase
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "DefaultDatabase must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.DefaultDatabase)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DefaultDatabaseIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            DefaultDatabase = "!nv@l!dDB"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "DefaultDatabase must be an alphanumeric string between 1 and 100 characters.";

        string error = validationResult[key: nameof(_connectionParameters.DefaultDatabase)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DatabaseIsNullOrEmpty(string? database)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Database = database
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Database must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.Database)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DatabaseIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Database = "!nv@l!dDB"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "Database must be an alphanumeric string between 1 and 100 characters.";
        string error = validationResult[key: nameof(_connectionParameters.Database)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIsNullOrEmpty(string? user)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            User = user
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "User must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.User)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            User = "123InvalidUser"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "User must be a valid alphanumeric string starting with a letter and between 3 and 30 characters.";

        string error = validationResult[key: nameof(_connectionParameters.User)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PasswordIsNullOrEmpty(string? password)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Password = password
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "Password must be provided and cannot be empty.";
        string error = validationResult[key: nameof(_connectionParameters.Password)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PasswordIsInvalid()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Password = "weakpass"
        };

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();

        const string expectedValidationMessage =
            "Password must be between 8 and 30 characters, contain an upper and lower case letter, a digit, and a special character, with no spaces.";

        string error = validationResult[key: nameof(_connectionParameters.Password)];
        error.Should().Be(expected: expectedValidationMessage);
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ConnectionParametersAreValid()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(settings: _connectionParameters);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }
}