using Expenso.Shared.System.Configuration.Exceptions;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

internal sealed class Validate : SettingsServiceTestBase
{
    [Test]
    public void Validate_Should_BeSuccessful()
    {
        // Arrange
        TestCandidate.Bind(sectionName: "TestSection");
        Mock<ISettingsValidator<TestSettings>> validatorMock = new();

        validatorMock
            .Setup(expression: v => v.Validate(It.IsAny<TestSettings>()))
            .Returns(value: new Dictionary<string, string>());

        _validatorsMock
            .Setup(expression: v => v.GetEnumerator())
            .Returns(value: new List<ISettingsValidator<TestSettings>>
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

        Dictionary<string, string> errors = new()
        {
            { "TestKey", "TestError" }
        };

        Mock<ISettingsValidator<TestSettings>> validatorMock = new();
        validatorMock.Setup(expression: v => v.Validate(It.IsAny<TestSettings>())).Returns(value: errors);

        _validatorsMock
            .Setup(expression: v => v.GetEnumerator())
            .Returns(value: new List<ISettingsValidator<TestSettings>>
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
                It.IsAny<SettingsValidationException>(), null, typeof(TestSettings).Name, "TestKey: TestError"),
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
            .WithMessage(expectedWildcardPattern: "Settings of type TestSettings have not been bound yet");

        _loggerMock.Verify(
            expression: l => l.LogError(LoggingUtils.ConfigurationError,
                "Settings of type {SettingsType} have not been bound yet",
                It.IsAny<SettingsHasNotBeenBoundYetException>(), null, typeof(TestSettings).Name), times: Times.Once);
    }
}