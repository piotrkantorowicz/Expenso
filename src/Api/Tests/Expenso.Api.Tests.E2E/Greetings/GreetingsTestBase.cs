namespace Expenso.Api.Tests.E2E.Greetings;

internal abstract class GreetingsTestBase
{
    protected HttpClient HttpClient { get; private set; } = null!;

    protected TestAuth TestAuth { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        HttpClient = WebApp.Instance.HttpClient;
        TestAuth = WebApp.Instance.TestAuth;
    }
}