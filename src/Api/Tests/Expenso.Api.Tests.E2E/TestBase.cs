using System.Net;

using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.E2E.TestData;
using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E;

[TestFixture]
internal abstract class TestBase
{
    public static readonly Dictionary<string, object?> Claims = new()
    {
        { ClaimNames.UserIdClaimName, TestClient.ClientId },
        { ClaimNames.UsernameClaimName, TestClient.ClientName }
    };

    [SetUp]
    public virtual Task SetUpAsync()
    {
        MessageContextFactoryMock = new Mock<IMessageContextFactory>();

        MessageContextFactoryMock
            .Setup(expression: x => x.Current(It.IsAny<Guid?>(), It.IsAny<string?>()))
            .Returns(value: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: Guid.CreateVersion7(),
                requestedBy: Guid.CreateVersion7(), timestamp: DateTimeOffset.Now, module: "TestModule"));

        _httpClient = WebApp.Instance.GetHttpClient();

        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDownAsync()
    {
        MessageContextFactoryMock = null!;
        WebApp.Instance.DestroyHttpClient();

        return Task.CompletedTask;
    }

    protected Mock<IMessageContextFactory> MessageContextFactoryMock { get; set; } = null!;

    protected HttpClient _httpClient = null!;

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