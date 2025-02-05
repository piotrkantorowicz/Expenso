using System.Net;

using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E;

[TestFixture]
internal abstract class TestBase
{
    private IServiceScope _serviceScope;
    private Mock<IMessageContextFactory> _messageContextFactoryMock;

    [SetUp]
    public virtual Task SetUpAsync()
    {
        _messageContextFactoryMock = new Mock<IMessageContextFactory>();

        _messageContextFactoryMock
            .Setup(expression: x => x.Current(It.IsAny<Guid?>(), It.IsAny<string?>()))
            .Returns(value: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: Guid.CreateVersion7(),
                requestedBy: Guid.CreateVersion7(), timestamp: DateTimeOffset.Now, module: "TestModule"));

        _serviceScope = WebApp.Instance.ServiceProvider.CreateScope();
        _claimsService = _serviceScope.ServiceProvider.GetRequiredService<ClaimsService>();
        _httpClient = _serviceScope.ServiceProvider.GetRequiredService<HttpClient>();

        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDownAsync()
    {
        _serviceScope.Dispose();
        _messageContextFactoryMock.Reset();
        _claimsService = null!;
        _httpClient = null!;
        _messageContextFactoryMock = null!;
        _serviceScope = null!;

        return Task.CompletedTask;
    }

    protected HttpClient _httpClient = null!;
    protected ClaimsService _claimsService = null!;

    protected static void AssertResponseStatusCode(HttpResponseMessage response, HttpStatusCode statusCode)
    {
        response.StatusCode.ShouldBe(expected: statusCode);
    }

    protected static void AssertCorrelationIdHeader(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();

        string? correlationIdHeader =
            response.Headers.GetValues(name: CorrelationIdMiddleware.CorrelationHeaderKey).FirstOrDefault();

        correlationIdHeader.ShouldNotBeEmpty();
        correlationIdHeader.ShouldBeOfType<string>();
        Guid.TryParse(input: correlationIdHeader, result: out _).ShouldBeTrue();
    }

    protected static void AssertModuleIdHeader(HttpResponseMessage response, string moduleName)
    {
        response.Headers.Contains(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).ShouldBeTrue();

        string? moduleIdHeaderValue =
            response.Headers.GetValues(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).FirstOrDefault();

        moduleIdHeaderValue.ShouldNotBeEmpty();
        moduleIdHeaderValue.ShouldBe(expected: moduleName);
    }

    protected static void AssertTimezoneIdHeader(HttpResponseMessage response)
    {
        response.Headers.Contains(name: RequestTimeZoneHeaderProvider.DefaultHeader).ShouldBeTrue();

        string? timeZoneHeaderValue =
            response.Headers.GetValues(name: RequestTimeZoneHeaderProvider.DefaultHeader).FirstOrDefault();

        timeZoneHeaderValue.ShouldNotBeEmpty();
        timeZoneHeaderValue.ShouldBe(expected: TimeZoneIds.Utc);
    }

    protected static void AssertNoModuleHeader(HttpResponseMessage response)
    {
        response.Headers.Contains(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).ShouldBeFalse();
    }

    protected static void AssertNoCorrelationIdModuleHeader(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeFalse();
    }
}