using Expenso.Api.Configuration.Auth.Claims;

namespace Expenso.Api.Tests.E2E;

internal abstract class TestBase
{
    protected const string? Username = "Test user";
    private static readonly Guid UserId = new("a8033772-3f5a-4127-8d23-de01aaf4f5d9");

    protected readonly Dictionary<string, object?> _claims = new()
    {
        { ClaimNames.UserIdClaimName, UserId },
        { ClaimNames.UsernameClaimName, Username }
    };

    protected HttpClient _httpClient = null!;

    [SetUp]
    public virtual Task SetUp()
    {
        _httpClient = WebApp.Instance.GetHttpClient();

        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDown()
    {
        WebApp.Instance.DestroyHttpClient();

        return Task.CompletedTask;
    }
}