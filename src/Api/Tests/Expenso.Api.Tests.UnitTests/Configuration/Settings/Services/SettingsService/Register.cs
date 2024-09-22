using Expenso.Shared.System.Configuration.Exceptions;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.SettingsService;

internal sealed class Register : SettingsServiceTestBase
{
    [Test]
    public void Register_Should_RegisterSettings_When_NotAlreadyRegistered()
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

        TestCandidate.Validate();

        // Act
        TestCandidate.Register(serviceCollection: _serviceCollection);

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogInfo(LoggingUtils.ConfigurationInformation,
                "Settings of type {SettingsType} are being registered as a singleton in the service collection", null,
                nameof(TestSettings)), times: Times.Once);

        _serviceCollection.Should().Contain(predicate: x => x.ServiceType == typeof(TestSettings));
    }

    [Test]
    public void Register_Should_ThrowSettingsHasNotBeenValidatedYetException_When_NotValidated()
    {
        // Act
        Action action = () => TestCandidate.Register(serviceCollection: _serviceCollection);

        // Assert
        action
            .Should()
            .Throw<SettingsHasNotBeenValidatedYetException>()
            .WithMessage(expectedWildcardPattern: "Settings of type TestSettings have not been validated yet.");

        _loggerMock.Verify(
            expression: l => l.LogError(LoggingUtils.ConfigurationError,
                "Settings of type {SettingsType} have not been validated yet",
                It.IsAny<SettingsHasNotBeenValidatedYetException>(), null, nameof(TestSettings)),
            times: Times.Once);
    }

    [Test]
    public void Register_Should_LogWarning_When_SettingsAlreadyRegistered()
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

        TestCandidate.Validate();
        _serviceCollection.AddSingleton<TestSettings>();

        // Act
        TestCandidate.Register(serviceCollection: _serviceCollection);

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "Settings of type {SettingsType} have already been registered in the service collection. Skipping registration",
                null, null, nameof(TestSettings)), times: Times.Once);
    }
}