using System.Reflection;

using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

using TestCandidate = Expenso.Api.Configuration.AppConfigurationManager;

namespace Expenso.Api.Tests.UnitTests.Configuration.AppConfigurationManager;

internal sealed class Configure : AppConfigurationManagerTestBase
{
    [Test]
    public void Should_AddSettingsFromBinders()
    {
        // Arrange
        object settings = new
        {
            Name = "settings"
        };

        _settingsBinderMock.Setup(expression: b => b.Bind(_serviceCollectionMock.Object)).Returns(value: settings);
        _settingsBinderMock.Setup(expression: x => x.GetSectionName()).Returns(value: "TestSection");

        List<ISettingsBinder?> binders = new()
        {
            _settingsBinderMock.Object
        };

        _preStartupContainerMock.Setup(expression: c => c.ResolveMany<ISettingsBinder>()).Returns(value: binders!);

        // Act
        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Assert
        FieldInfo? settingsMap = typeof(TestCandidate).GetField(name: "_settingsMap",
            bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);

        Dictionary<string, object?> result =
            (Dictionary<string, object?>)settingsMap?.GetValue(obj: _appConfigurationManager)!;

        result.Should().ContainKey(expected: "TestSection").WhoseValue.Should().Be(expected: settings);
    }

    [Test]
    public void Should_LogWarningAndNotWhen_AlreadyConfigured()
    {
        // Arrange
        _settingsBinderMock
            .Setup(expression: b => b.Bind(_serviceCollectionMock.Object))
            .Returns(value: new
            {
                Name = "settings"
            });

        _settingsBinderMock.Setup(expression: x => x.GetSectionName()).Returns(value: "TestSection");

        _preStartupContainerMock
            .Setup(expression: c => c.ResolveMany<ISettingsBinder>())
            .Returns(value: new List<ISettingsBinder?>
            {
                _settingsBinderMock.Object
            }!);

        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Act
        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "Settings have already been configured. Exiting configuration", null, null), times: Times.Once);
    }

    [Test]
    public void Should_LogWarning_When_NoSettingsBinderFound()
    {
        // Arrange
        _preStartupContainerMock
            .Setup(expression: c => c.ResolveMany<ISettingsBinder>())
            .Returns(value: new List<ISettingsBinder?>()!);

        // Act
        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning, "No ISettingsBinder services were found",
                null, null), times: Times.Once);
    }

    [Test]
    public void Should_SkipNullBinderAndLogWarning()
    {
        // Arrange
        List<ISettingsBinder?> binders = new()
        {
            null,
            _settingsBinderMock.Object
        };

        _preStartupContainerMock.Setup(expression: c => c.ResolveMany<ISettingsBinder>()).Returns(value: binders!);

        // Act
        _appConfigurationManager.Configure(serviceCollection: _serviceCollectionMock.Object);

        // Assert
        _loggerMock.Verify(
            expression: l => l.LogWarning(LoggingUtils.ConfigurationWarning,
                "A null binder was encountered and skipped", null, null), times: Times.Once);
    }
}