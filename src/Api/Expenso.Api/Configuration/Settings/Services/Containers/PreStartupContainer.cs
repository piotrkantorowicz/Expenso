using System.Reflection;

using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Logging.Serilog;
using Expenso.Shared.System.Metrics;

using Serilog;

namespace Expenso.Api.Configuration.Settings.Services.Containers;

internal sealed class PreStartupContainer : IPreStartupContainer, IDisposable, IAsyncDisposable
{
    private ILoggerService<PreStartupContainer>? _logger;
    private ServiceProvider? _serviceProvider;

    public async ValueTask DisposeAsync()
    {
        if (_serviceProvider is null)
        {
            return;
        }

        await _serviceProvider.DisposeAsync();
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }

    public void Build(IConfiguration configuration, IEnumerable<Assembly> assemblies)
    {
        ServiceCollection serviceCollection = [];
        serviceCollection.AddSingleton(implementationInstance: configuration);
        serviceCollection.AddSettings(assemblies: assemblies);

        serviceCollection.AddSingleton(implementationInstance: new ApplicationSettings
        {
            Name = "Configurator",
            Version = "0.0.0"
        });

        serviceCollection.AddInternalLogging();
        OtlpSettings? settings = configuration.GetSection(key: SectionNames.Otlp).Get<OtlpSettings>();

        serviceCollection.AddLogging(configure: loggingBuilder => loggingBuilder.AddSerilog(dispose: true,
            logger: new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteToOpenTelemetry(otlpEndpoint: settings?.Endpoint, otlpService: settings?.ServiceName)
                .CreateLogger()));

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILoggerService<PreStartupContainer>>();

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Configuration ServiceProvider has been loaded successfully");
    }

    public T? Resolve<T>()
    {
        if (_serviceProvider is not null)
        {
            return _serviceProvider.GetService<T>();
        }

        _logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
            message: "Service provider is not initialized. Unable to resolve service of type {ServiceType}",
            exception: null, messageContext: null, typeof(T).Name);

        return default;
    }

    public IReadOnlyCollection<T> ResolveMany<T>()
    {
        if (_serviceProvider is not null)
        {
            return _serviceProvider.GetServices<T>().ToList().AsReadOnly();
        }

        _logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
            message: "Service provider is not initialized. Unable to resolve service of type {ServiceType}",
            exception: null, messageContext: null, typeof(T).Name);

        return [];
    }
}