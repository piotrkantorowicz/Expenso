using System.Net;

using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.E2E.TestData;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E;

[TestFixture]
internal abstract class TestBase
{
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

    protected readonly Dictionary<string, object?> _claims = new()
    {
        { ClaimNames.UserIdClaimName, TestClient.ClientId },
        { ClaimNames.UsernameClaimName, TestClient.ClientName }
    };

    protected HttpClient _httpClient = null!;

    protected virtual void AssertResponseOk(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();
        response.StatusCode.ShouldBe(expected: HttpStatusCode.OK);
    }

    protected virtual void AssertResponseCreated(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();
        response.StatusCode.ShouldBe(expected: HttpStatusCode.Created);
    }

    protected virtual void AssertResponseNoContent(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();
        response.StatusCode.ShouldBe(expected: HttpStatusCode.NoContent);
    }

    protected virtual void AssertResponseBadRequest(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();
        response.StatusCode.ShouldBe(expected: HttpStatusCode.BadRequest);
    }

    protected static void AssertResponseUnauthroised(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(expected: HttpStatusCode.Unauthorized);
    }

    protected static void AssertModuleHeader(HttpResponseMessage response, string moduleName)
    {
        response.Headers.Contains(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).ShouldBeTrue();

        string? moduleIdHeaderValue =
            response.Headers.GetValues(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).FirstOrDefault();

        moduleIdHeaderValue.ShouldNotBeEmpty();
        moduleIdHeaderValue.ShouldBe(expected: moduleName);
    }
}