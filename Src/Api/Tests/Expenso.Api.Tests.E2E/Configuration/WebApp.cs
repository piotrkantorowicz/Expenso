namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class WebApp
{
    private static readonly Lazy<WebApp> Lazy = new(() => new WebApp());
    private readonly ExpensoWebApplication _expensoWebApplication;

    private WebApp()
    {
        ExpensoWebApplication app = new();
        HttpClient client = app.CreateClient();
        _expensoWebApplication = app;
        ServiceProvider = app.Services;
        HttpClient = client;
    }

    public static WebApp Instance => Lazy.Value;

    public HttpClient? HttpClient { get; private set; }

    public IServiceProvider ServiceProvider { get; }

    public void SetNewHttpClient()
    {
        HttpClient client = _expensoWebApplication.CreateClient();
        HttpClient = client;
    }

    public void DestroyHttpClient()
    {
        HttpClient?.Dispose();
        HttpClient = null!;
    }
    
    public void Destroy()
    {
        HttpClient?.Dispose();
        _expensoWebApplication.Dispose();
    }
}