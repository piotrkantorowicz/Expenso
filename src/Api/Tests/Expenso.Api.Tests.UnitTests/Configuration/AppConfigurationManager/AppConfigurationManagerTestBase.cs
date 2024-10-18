using Expenso.Api.Configuration.Settings.Services.Containers;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.UnitTests.Configuration.AppConfigurationManager;

internal abstract class AppConfigurationManagerTestBase
{
    protected Api.Configuration.AppConfigurationManager _appConfigurationManager = null!;
    protected Mock<ILoggerService<Api.Configuration.AppConfigurationManager>> _loggerMock = null!;
    protected Mock<IPreStartupContainer> _preStartupContainerMock = null!;
    protected Mock<IServiceCollection> _serviceCollectionMock = null!;
    protected Mock<ISettingsBinder> _settingsBinderMock = null!;

    [SetUp]
    public void SetUp()
    {
        _preStartupContainerMock = new Mock<IPreStartupContainer>();
        _loggerMock = new Mock<ILoggerService<Api.Configuration.AppConfigurationManager>>();
        _serviceCollectionMock = new Mock<IServiceCollection>();
        _settingsBinderMock = new Mock<ISettingsBinder>();

        _preStartupContainerMock
            .Setup(expression: c => c.Resolve<ILoggerService<Api.Configuration.AppConfigurationManager>>())
            .Returns(value: _loggerMock.Object);

        _appConfigurationManager =
            new Api.Configuration.AppConfigurationManager(preStartupContainer: _preStartupContainerMock.Object);
    }
}