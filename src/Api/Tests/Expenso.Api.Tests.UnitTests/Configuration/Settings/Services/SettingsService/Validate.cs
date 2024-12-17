using Expenso.Shared.System.Configuration.Exceptions;
using Expenso.Shared.System.Logging.Constants;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Moq;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

[TestFixture]
internal sealed class Validate : SettingsServiceTestBase
{
    [Test]
    public void Validate_Should_BeSuccessful()
    {
        // Arrange
        TestCandidate.Bind(sectionName: "TestSection");
        Mock<IValidator<TestSettings>> validatorMock = new();

        validatorMock
            .Setup(expression: v => v.Validate(It.IsAny<TestSettings>()))
            .Returns(value: new ValidationResult());

        _validatorsMock
            .Setup(expression: v => v.GetEnumerator())
            .Returns(value: new List<IValidator<TestSettings>>
            {
                validatorMock.Object
            }.GetEnumerator());

        // Act
        TestCandidate.Validate();

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogInfo(LoggingUtils.ConfigurationInformation,
                "Settings of type {SettingsType} have been successfully validated", null, nameof(TestSettings)),
            times: Times.Once);
    }

    [Test]
    public void Validate_Should_ThrowSettingsValidationException_When_ValidationFails()
    {
        // Arrange
        TestCandidate.Bind(sectionName: "TestSection");

        ValidationResult validationResult = new()
        {
            Errors =
            {
                new ValidationFailure(propertyName: "TestKey", errorMessage: "TestError")
            }
        };

        Mock<IValidator<TestSettings>> validatorMock = new();
        validatorMock.Setup(expression: v => v.Validate(It.IsAny<TestSettings>())).Returns(value: validationResult);

        _validatorsMock
            .Setup(expression: v => v.GetEnumerator())
            .Returns(value: new List<IValidator<TestSettings>>
            {
                validatorMock.Object
            }.GetEnumerator());

        // Act
        Action action = () => TestCandidate.Validate();

        // Assert
        action
            .Should()
            .Throw<SettingsValidationException>()
            .Which.ErrorDictionary.Should()
            .ContainKey(expected: "TestKey")
            .WhoseValue.Should()
            .Be(expected: "TestError");

        _loggerMock.Verify(
            expression: l => l.LogError(LoggingUtils.ConfigurationError,
                "Validation failed for settings of type {SettingsType}. Errors: {ValidationErrors}",
                It.IsAny<SettingsValidationException>(), null, nameof(TestSettings), "TestKey: TestError"),
            times: Times.Once);
    }

    [Test]
    public void Validate_Should_ThrowSettingsHasNotBeenBoundYetException_When_NotBound()
    {
        // Act
        Action action = () => TestCandidate.Validate();

        // Assert
        action
            .Should()
            .Throw<SettingsHasNotBeenBoundYetException>()
            .WithMessage(expectedWildcardPattern: "Settings of type TestSettings have not been bound yet.");

        _loggerMock.Verify(
            expression: l => l.LogError(LoggingUtils.ConfigurationError,
                "Settings of type {SettingsType} have not been bound yet",
                It.IsAny<SettingsHasNotBeenBoundYetException>(), null, nameof(TestSettings)), times: Times.Once);
    }
}