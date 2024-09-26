using Expenso.Api.Configuration.Settings;
using Expenso.Shared.System.Configuration.Sections;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Expenso.Api.Tests.UnitTests.Configuration.Builders.AppBuilder;

internal sealed class ConfigureCors : AppBuilderTestBase
{
    [Test]
    public void ConfigureCors_Should_AddCors_When_EnabledIsTrue()
    {
        // Arrange
        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: _corsSettings);

        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .Should()
            .NotBeNull();
    }

    [Test]
    public void ConfigureCors_Should_NotAddCors_When_EnabledIsFalse()
    {
        // Arrange
        CorsSettings corsSettings = _corsSettings with
        {
            Enabled = false
        };

        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: corsSettings);

        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .Should()
            .BeNull();
    }

    [Test, TestCase(arg: "Production"), TestCase(arg: "Staging"), TestCase(arg: "UAT")]
    public void ConfigureCors_Should_UseAllowedOrigins_When_NotInDevelopmentOrLocalOrTest(string environmentName)
    {
        // Arrange
        _webApplicationBuilder = WebApplication.CreateBuilder(options: new WebApplicationOptions
        {
            EnvironmentName = environmentName
        });

        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: _corsSettings);

        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .Should()
            .NotBeNull();

        AssertCorsPolicy(origin: _corsSettings.AllowedOrigins);
    }

    [Test, TestCase(arg: "Local"), TestCase(arg: "Development"), TestCase(arg: "Test")]
    public void ConfigureCors_Should_NotUseAllowedOrigins_When_InDevelopmentOrLocalOrTest(string environmentName)
    {
        // Arrange
        _webApplicationBuilder = WebApplication.CreateBuilder(options: new WebApplicationOptions
        {
            EnvironmentName = environmentName
        });

        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: _corsSettings);

        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .Should()
            .NotBeNull();

        AssertCorsPolicy();
    }

    private void AssertCorsPolicy(string[]? origin = null)
    {
        ServiceProvider serviceProvider = _serviceCollection.BuildServiceProvider();
        CorsOptions corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;
        CorsPolicy? defaultPolicy = corsOptions.GetPolicy(name: "__DefaultCorsPolicy");
        defaultPolicy?.Should().NotBeNull();

        if (origin is null)
        {
            defaultPolicy?.Origins.Should().BeEmpty();
        }
        else
        {
            defaultPolicy?.Origins.Should().Contain(expected: origin);
        }

        defaultPolicy?.Methods.Should().Contain(expected: "*");
        defaultPolicy?.Headers.Should().Contain(expected: "*");
        defaultPolicy?.SupportsCredentials.Should().BeTrue();
    }
}