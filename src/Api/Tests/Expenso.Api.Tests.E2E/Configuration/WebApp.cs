namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class WebApp
{
    private static readonly Lazy<WebApp> Lazy = new(valueFactory: () => new WebApp());
    private readonly ExpensoWebApplication _expensoWebApplication;

    private WebApp()
    {
        ExpensoWebApplication app = new();
        _expensoWebApplication = app;
        ServiceProvider = app.Services;
    }

    public static WebApp Instance => Lazy.Value;

    public IServiceProvider ServiceProvider { get; }

    public HttpClient GetHttpClient()
    {
        return _expensoWebApplication.CreateClient();
    }

    public void Destroy()
    {
        _expensoWebApplication.Dispose();
    }
}