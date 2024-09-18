using Expenso.Api.Configuration.Settings.Services.Containers;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Api.Configuration.AppConfigurationManager;

namespace Expenso.Api.Tests.UnitTests.AppConfigurationManager;

internal abstract class AppConfigurationManagerTestBase
{
    protected TestCandidate _appConfigurationManager = null!;
    protected Mock<ILoggerService<TestCandidate>> _loggerMock = null!;
    protected Mock<IPreStartupContainer> _preStartupContainerMock = null!;
    protected Mock<IServiceCollection> _serviceCollectionMock = null!;
    protected Mock<ISettingsBinder> _settingsBinderMock = null!;

    [SetUp]
    public void SetUp()
    {
        _preStartupContainerMock = new Mock<IPreStartupContainer>();
        _loggerMock = new Mock<ILoggerService<TestCandidate>>();
        _serviceCollectionMock = new Mock<IServiceCollection>();
        _settingsBinderMock = new Mock<ISettingsBinder>();

        _preStartupContainerMock
            .Setup(expression: c => c.Resolve<ILoggerService<TestCandidate>>())
            .Returns(value: _loggerMock.Object);

        _appConfigurationManager = new TestCandidate(preStartupContainer: _preStartupContainerMock.Object);
    }
}