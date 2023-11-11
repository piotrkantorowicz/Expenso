using Expenso.Api.Configuration.Options;
using Microsoft.Extensions.Configuration;

namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class WebApp
{
    private static readonly Lazy<WebApp> Lazy = new(() => new WebApp());

    private readonly ExpensoWebApplication _expensoWebApplication;

    private WebApp()
    {
        ExpensoWebApplication app = new();
        HttpClient client = app.CreateClient();
        IConfiguration configuration = (IConfiguration)app.Services.GetService(typeof(IConfiguration))!;

        configuration.TryBindOptions(nameof(TestAuth), out TestAuth testAuth);

        _expensoWebApplication = app;
        HttpClient = client;
        TestAuth = testAuth;
    }

    public static WebApp Instance => Lazy.Value;

    public HttpClient HttpClient { get; }

    public TestAuth TestAuth { get; }

    public void Destroy()
    {
        _expensoWebApplication.Dispose();
        HttpClient.Dispose();
    }
}