using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Api.Tests.E2E;

[TestFixture]
internal abstract class TestBase
{
    [SetUp]
    public virtual Task SetUpAsync()
    {
        MessageContextFactoryMock = new Mock<IMessageContextFactory>();
        MessageContextFactoryMock.Setup(expression: x => x.Current(It.IsAny<Guid?>())).Returns(value: _messageContext);
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

    private readonly MessageContext _messageContext = new()
    {
        MessageId = Guid.NewGuid(),
        CorrelationId = Guid.NewGuid(),
        RequestedBy = Guid.NewGuid(),
        Timestamp = DateTimeOffset.Now,
        ModuleId = "TestModule"
    };

    protected Mock<IMessageContextFactory> MessageContextFactoryMock { get; set; } = null!;

    protected const string? Username = "Test user";
    private static readonly Guid UserId = new(g: "a8033772-3f5a-4127-8d23-de01aaf4f5d9");

    protected readonly Dictionary<string, object?> _claims = new()
    {
        { ClaimNames.UserIdClaimName, UserId },
        { ClaimNames.UsernameClaimName, Username }
    };

    protected HttpClient _httpClient = null!;

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