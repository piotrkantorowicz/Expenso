using Expenso.Api.Configuration.Settings.ApiSettings;
using Expenso.Shared.System.Configuration.Constants;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.UnitTests.Configuration.Builders.AppBuilder;

[TestFixture]
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
            .ShouldNotBeNull();
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
        _serviceCollection.FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider)).ShouldBeNull();
    }

    [Test, TestCase(arg: "Production"), TestCase(arg: "Staging"), TestCase(arg: "UAT")]
    public void ConfigureCors_Should_UseAllowedOrigins_When_NotInDevelopmentOrLocalOrTest(string environmentName)
    {
        // Arrange
        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: _corsSettings);

        SetupWebApplicationBuilder(environmentName: environmentName);
        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .ShouldNotBeNull();

        AssertCorsPolicy(origins: _corsSettings.AllowedOrigins);
    }

    [Test, TestCase(arg: "Local"), TestCase(arg: "Development"), TestCase(arg: "Test")]
    public void ConfigureCors_Should_NotUseAllowedOrigins_When_InDevelopmentOrLocalOrTest(string environmentName)
    {
        // Arrange
        _configurationManagerMock
            .Setup(expression: x => x.GetSettings<CorsSettings>(SectionNames.Cors))
            .Returns(value: _corsSettings);

        SetupWebApplicationBuilder(environmentName: environmentName);
        CreateTestCandiate();

        // Act
        TestCandidate.ConfigureCors();

        // Assert
        _serviceCollection
            .FirstOrDefault(predicate: d => d.ServiceType == typeof(ICorsPolicyProvider))
            .ShouldNotBeNull();

        AssertCorsPolicy();
    }

    private void AssertCorsPolicy(string[]? origins = null)
    {
        ServiceProvider serviceProvider = _serviceCollection.BuildServiceProvider();
        CorsOptions corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;
        CorsPolicy? defaultPolicy = corsOptions.GetPolicy(name: "__DefaultCorsPolicy");
        defaultPolicy?.ShouldNotBeNull();

        if (origins is null)
        {
            defaultPolicy?.Origins.ShouldBeEmpty();
        }
        else
        {
            foreach (string origin in origins)
            {
                defaultPolicy?.Origins.ShouldContain(expected: origin);
            }
        }

        defaultPolicy?.Methods.ShouldContain(expected: "*");
        defaultPolicy?.Headers.ShouldContain(expected: "*");
        defaultPolicy?.SupportsCredentials.ShouldBeTrue();
    }

    private void SetupWebApplicationBuilder(string? environmentName = null)
    {
        _webApplicationBuilder = WebApplication.CreateBuilder(options: new WebApplicationOptions
        {
            EnvironmentName = environmentName ?? _webApplicationBuilder.Environment.EnvironmentName
        });
    }
}