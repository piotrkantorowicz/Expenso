namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class WebApp
{
    private static readonly Lazy<WebApp> Lazy = new(valueFactory: () => new WebApp());
    private readonly ExpensoWebApplication _expensoWebApplication;
    private HttpClient? _httpClient;

    private WebApp()
    {
        ExpensoWebApplication app = new();
        HttpClient client = app.CreateClient();
        _expensoWebApplication = app;
        ServiceProvider = app.Services;
        _httpClient = client;
    }

    public static WebApp Instance => Lazy.Value;

    public IServiceProvider ServiceProvider { get; }

    public HttpClient GetHttpClient()
    {
        _httpClient = _expensoWebApplication.CreateClient();

        return _httpClient;
    }

    public void DestroyHttpClient()
    {
        _httpClient?.Dispose();
        _httpClient = null;
    }

    public void Destroy()
    {
        _httpClient?.Dispose();
        _expensoWebApplication.Dispose();
    }
}