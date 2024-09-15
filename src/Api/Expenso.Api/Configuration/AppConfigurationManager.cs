using System.Reflection;

using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Logging.Serilog;
using Expenso.Shared.System.Metrics;

using Serilog;

namespace Expenso.Api.Configuration;

internal sealed class AppConfigurationManager
{
    private readonly ILoggerService<AppConfigurationManager> _logger;
    private readonly Dictionary<string, object?> _settingsMap = new();
    private bool _configured;
    private ServiceProvider? _serviceProvider;

    public AppConfigurationManager(IEnumerable<Assembly> assemblies, IConfiguration configuration)
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
        _logger = _serviceProvider.GetRequiredService<ILoggerService<AppConfigurationManager>>();

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Configuration ServiceProvider has been loaded successfully");
    }

    public TSettings? GetSettings<TSettings>(string sectionName) where TSettings : class
    {
        object? settings = _settingsMap[key: sectionName];

        return settings as TSettings;
    }

    public void Configure(IServiceCollection serviceCollection)
    {
        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation, message: "Starting configuration of settings");

        if (_configured)
        {
            _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "Settings have already been configured. Exiting configuration");

            return;
        }

        IEnumerable<object?>? binders = _serviceProvider?.GetServices(serviceType: typeof(ISettingsBinder));

        if (binders is null)
        {
            _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "No ISettingsBinder services were found");

            return;
        }

        foreach (object? binder in binders)
        {
            if (binder is null)
            {
                _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "A null binder was encountered and skipped");

                continue;
            }

            Type binderType = binder.GetType();
            MethodInfo? getSectionNameMethod = binderType.GetMethod(name: "GetSectionName");
            MethodInfo? bindMethod = binderType.GetMethod(name: "Bind");

            if (getSectionNameMethod == null || bindMethod == null)
            {
                _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "Binder of type {BinderType} does not have the expected methods", exception: null,
                    messageContext: null, binderType.Name);

                continue;
            }

            string? sectionName = getSectionNameMethod.Invoke(obj: binder, parameters: null) as string;

            object? boundSettings = bindMethod.Invoke(obj: binder, parameters: new object[]
            {
                serviceCollection
            });

            if (sectionName != null)
            {
                _settingsMap.Add(key: sectionName, value: boundSettings);

                _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
                    message: "Settings for section {SectionName} have been bound successfully", messageContext: null,
                    sectionName);
            }
            else
            {
                _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "Section name returned null for binder of type {BinderType}", exception: null,
                    messageContext: null, binderType.Name);
            }
        }

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Configuration process completed. Disposing the service provider");

        _configured = true;
        _serviceProvider?.Dispose();
        _serviceProvider = null;
    }
}