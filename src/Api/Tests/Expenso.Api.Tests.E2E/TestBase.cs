using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Execution.Middlewares;

namespace Expenso.Api.Tests.E2E;

internal abstract class TestBase
{
    protected const string? Username = "Test user";
    private static readonly Guid UserId = new(g: "a8033772-3f5a-4127-8d23-de01aaf4f5d9");

    protected readonly Dictionary<string, object?> _claims = new()
    {
        { ClaimNames.UserIdClaimName, UserId },
        { ClaimNames.UsernameClaimName, Username }
    };

    protected HttpClient _httpClient = null!;

    [SetUp]
    public virtual Task SetUpAsync()
    {
        _httpClient = WebApp.Instance.GetHttpClient();

        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDownAsync()
    {
        WebApp.Instance.DestroyHttpClient();

        return Task.CompletedTask;
    }

    protected virtual void AssertResponseOk(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        response.StatusCode.Should().Be(expected: HttpStatusCode.OK);
    }

    protected virtual void AssertResponseCreated(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        response.StatusCode.Should().Be(expected: HttpStatusCode.Created);
    }

    protected virtual void AssertResponseNoContent(HttpResponseMessage response)
    {
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        response.StatusCode.Should().Be(expected: HttpStatusCode.NoContent);
    }

    protected static void AssertResponseUnauthroised(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }

    protected void AssertModuleHeader(HttpResponseMessage response, string moduleName)
    {
        response.Headers.Contains(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).Should().BeTrue();

        string? moduleIdHeaderValue =
            response.Headers.GetValues(name: ModuleIdMiddleware.ModuleMiddlewareHeaderKey).FirstOrDefault();

        moduleIdHeaderValue.Should().NotBeNullOrEmpty();
        moduleIdHeaderValue.Should().Be(expected: moduleName);
    }
}