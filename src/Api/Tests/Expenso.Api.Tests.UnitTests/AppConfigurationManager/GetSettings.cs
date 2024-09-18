using Expenso.Api.Configuration.Settings.Exceptions;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Tests.UnitTests.AppConfigurationManager;

internal sealed class GetSettings : AppConfigurationManagerTestBase
{
    [Test]
    public void Should_ReturnCorrectSettings()
    {
        // Arrange
        object expectedSettings = new
        {
            Enabled = false
        };

        _settingsBinderMock
            .Setup(expression: x => x.Bind(_serviceCollectionMock.Object))
            .Returns(value: expectedSettings);

        _settingsBinderMock.Setup(expression: x => x.GetSectionName()).Returns(value: "SectionName");

        _preStartupContainerMock
            .Setup(expression: x => x.ResolveMany<ISettingsBinder>())
            .Returns(value: [_settingsBinderMock.Object]);

        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Act
        object settings = _appConfigurationManager.GetSettings<object>(sectionName: "SectionName");

        // Assert
        settings.Should().BeSameAs(expected: expectedSettings);
    }

    [Test]
    public void Should_ThrowConfigurationHasNotBeenInitializedYetException_When_NotConfigured()
    {
        // Act
        Action action = () => _appConfigurationManager.GetSettings<object>(sectionName: "SectionName");

        // Assert
        action.Should().Throw<ConfigurationHasNotBeenInitializedYetException>();

        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "Configuration has not been initialized yet", null, null), times: Times.Once);
    }

    [Test]
    public void Should_ThrowMissingSettingsSectionException_When_SectionNotFound()
    {
        // Arrange
        _settingsBinderMock.Setup(expression: x => x.Bind(_serviceCollectionMock.Object)).Returns(value: new object());
        _settingsBinderMock.Setup(expression: x => x.GetSectionName()).Returns(value: "Section");

        _preStartupContainerMock
            .Setup(expression: x => x.ResolveMany<ISettingsBinder>())
            .Returns(value: [_settingsBinderMock.Object]);

        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Act
        Action action = () => _appConfigurationManager.GetSettings<object>(sectionName: "NonExistentSection");

        // Assert
        action
            .Should()
            .Throw<MissingSettingsSectionException>()
            .WithMessage(
                expectedWildcardPattern:
                "The settings section 'NonExistentSection' is missing. Please ensure the section is properly configured in the configuration file.");

        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "Settings not found for section: {SectionName}", null, null, "NonExistentSection"), times: Times.Once);
    }

    [Test]
    public void Should_ThrowInvalidSettingsTypeException_When_InvalidType()
    {
        // Arrange
        _settingsBinderMock.Setup(expression: x => x.Bind(_serviceCollectionMock.Object)).Returns(value: new object());
        _settingsBinderMock.Setup(expression: x => x.GetSectionName()).Returns(value: "SectionName");

        _preStartupContainerMock
            .Setup(expression: x => x.ResolveMany<ISettingsBinder>())
            .Returns(value: [_settingsBinderMock.Object]);

        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Act
        Action action = () => _appConfigurationManager.GetSettings<string>(sectionName: "SectionName");

        // Assert
        action.Should().Throw<InvalidSettingsTypeException>();
    }
}