using Expenso.Api.Configuration;
using Expenso.Api.Configuration.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Api.Configuration.Builders.AppBuilder;

namespace Expenso.Api.Tests.UnitTests.Builders.AppBuilder;

internal abstract class AppBuilderTestBase : TestBase<TestCandidate>
{
    protected readonly CorsSettings _corsSettings = new()
    {
        Enabled = true,
        AllowedOrigins = ["localhost:3000"]
    };

    protected Mock<IAppConfigurationManager> _configurationManagerMock;
    protected Mock<IConfiguration> _configurationMock;
    protected WebApplicationBuilder _webApplicationBuilder;
    protected IServiceCollection _serviceCollection;

    [SetUp]
    public void SetUp()
    {
        _webApplicationBuilder = WebApplication.CreateBuilder();
        _configurationManagerMock = new Mock<IAppConfigurationManager>();
        _serviceCollection = new ServiceCollection();
        _configurationMock = new Mock<IConfiguration>();
    }
}