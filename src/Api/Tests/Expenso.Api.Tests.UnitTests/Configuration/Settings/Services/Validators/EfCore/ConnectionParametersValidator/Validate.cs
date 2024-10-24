using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.ConnectionParametersValidator;

[TestFixture]
internal sealed class Validate : ConnectionParametersValidatorTestBase
{
    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "   ")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_HostIsNullOrWhiteSpace(string host)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Host = host
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Host),
            errorMessage: "Host must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Host),
            errorMessage: "Host must be a valid DNS name or IP address.");
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PortIsNullOrWhiteSpace()
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Port = string.Empty
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Port),
            errorMessage: "Port must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Port),
            errorMessage: "Port must be a valid integer between 1 and 65535.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DefaultDatabaseIsNullOrWhiteSpace(
        string? defaultDatabase)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            DefaultDatabase = defaultDatabase
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.DefaultDatabase),
            errorMessage: "DefaultDatabase must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.DefaultDatabase),
            errorMessage: "DefaultDatabase must be an alphanumeric string between 1 and 100 characters.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_DatabaseIsNullOrWhiteSpace(string? database)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Database = database
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Database),
            errorMessage: "Database must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Database),
            errorMessage: "Database must be an alphanumeric string between 1 and 100 characters.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIsNullOrWhiteSpace(string? user)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            User = user
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.User),
            errorMessage: "User must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.User),
            errorMessage:
            "User must be a valid alphanumeric string starting with a letter and between 3 and 30 characters.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PasswordIsNullOrWhiteSpace(string? password)
    {
        // Arrange
        _connectionParameters = _connectionParameters with
        {
            Password = password
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Password),
            errorMessage: "Password must be provided and cannot be empty.");
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
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(_connectionParameters.Password),
            errorMessage:
            "Password must be between 8 and 30 characters, contain an upper and lower case letter, a digit, and a special character, with no spaces.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ConnectionParametersAreValid()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _connectionParameters);

        // Assert
        validationResult.AssertNoErrors();
    }
}