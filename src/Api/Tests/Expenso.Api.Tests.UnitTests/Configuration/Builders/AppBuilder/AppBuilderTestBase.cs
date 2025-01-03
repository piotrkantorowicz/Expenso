using Expenso.Api.Configuration;
using Expenso.Api.Configuration.Settings.ApiSettings;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Builders.AppBuilder;

[TestFixture]
internal abstract class AppBuilderTestBase : TestBase<Api.Configuration.Builders.AppBuilder>
{
    [SetUp]
    public void SetUp()
    {
        _webApplicationBuilder = WebApplication.CreateBuilder();
        _configurationManagerMock = new Mock<IAppConfigurationManager>();
        _serviceCollection = new ServiceCollection();
        _configurationMock = new Mock<IConfiguration>();
    }

    [TearDown]
    public void TearDown()
    {
        _configurationManagerMock.Reset();
        _configurationMock.Reset();
        _webApplicationBuilder = null!;
        _configurationManagerMock = null!;
        _serviceCollection = null!;
        _configurationMock = null!;
    }

    protected readonly CorsSettings _corsSettings = new()
    {
        Enabled = true,
        AllowedOrigins = ["localhost:3000"]
    };

    protected Mock<IAppConfigurationManager> _configurationManagerMock = null!;
    private Mock<IConfiguration> _configurationMock = null!;
    protected IServiceCollection _serviceCollection = null!;
    protected WebApplicationBuilder _webApplicationBuilder = null!;

    protected void CreateTestCandiate()
    {
        TestCandidate = new Api.Configuration.Builders.AppBuilder(appBuilder: _webApplicationBuilder,
            configuration: _configurationMock.Object, serviceCollection: _serviceCollection,
            appConfigurationManager: _configurationManagerMock.Object);
    }
}