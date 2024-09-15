using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Exceptions;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Configuration.Settings;

internal sealed class SettingsService<TSettings> : ISettingsService<TSettings> where TSettings : class, ISettings, new()
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerService<SettingsService<TSettings>> _logger;
    private readonly IEnumerable<ISettingsValidator<TSettings>> _validators;
    private bool _binded;
    private TSettings _settings = null!;
    private bool _validated;

    public SettingsService(IEnumerable<ISettingsValidator<TSettings>> validators, IConfiguration configuration,
        ILoggerService<SettingsService<TSettings>> logger)
    {
        _validators = validators ?? throw new ArgumentNullException(paramName: nameof(validators));
        _configuration = configuration ?? throw new ArgumentNullException(paramName: nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public void Validate()
    {
        if (!_binded)
        {
            SettingsHasNotBeenBoundYetException exception = new(settingsName: typeof(TSettings).Name);

            _logger.LogError(eventId: LoggingUtils.ConfigurationError,
                message: "Settings of type {SettingsType} have not been bound yet", exception: exception,
                messageContext: null, typeof(TSettings).Name);

            throw exception;
        }

        Dictionary<string, string> errors = _validators
            .Select(selector: x => x.Validate(settings: _settings))
            .SelectMany(selector: x => x)
            .ToDictionary(keySelector: x => x.Key, elementSelector: x => x.Value);

        if (errors.Count > 0)
        {
            SettingsValidationException exception = new(errorDictionary: errors);

            _logger.LogError(eventId: LoggingUtils.ConfigurationError,
                message: "Validation failed for settings of type {SettingsType}. Errors: {ValidationErrors}",
                exception: exception, messageContext: null, typeof(TSettings).Name,
                string.Join(separator: ", ", values: errors.Select(selector: e => $"{e.Key}: {e.Value}")));

            throw exception;
        }

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Settings of type {SettingsType} have been successfully validated", messageContext: null,
            typeof(TSettings).Name);

        _validated = true;
    }

    public TSettings Bind(string sectionName)
    {
        if (_binded)
        {
            _logger.LogDebug(eventId: LoggingUtils.ConfigurationInformation,
                message: "Settings of type {SettingsType} have already been bound. Returning the current instance",
                messageContext: null, typeof(TSettings).Name);

            return _settings;
        }

        _configuration.TryBindOptions(sectionName: sectionName, options: out _settings);
        _binded = true;

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Settings of type {SettingsType} have been successfully bound from section {SectionName}",
            messageContext: null, typeof(TSettings).Name, sectionName);

        return _settings;
    }

    public void Register(IServiceCollection serviceCollection)
    {
        if (!_validated)
        {
            SettingsHasNotBeenValidatedYetException exception = new(settingsName: typeof(TSettings).Name);

            _logger.LogError(eventId: LoggingUtils.ConfigurationError,
                message: "Settings of type {SettingsType} have not been validated yet", exception: exception,
                messageContext: null, typeof(TSettings).Name);

            throw exception;
        }

        if (serviceCollection.Any(predicate: descriptor => descriptor.ServiceType == typeof(TSettings)))
        {
            _logger.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message:
                "Settings of type {SettingsType} have already been registered in the service collection. Skipping registration",
                exception: null, messageContext: null, typeof(TSettings).Name);

            return;
        }

        _logger.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
            message: "Settings of type {SettingsType} are being registered as a singleton in the service collection",
            messageContext: null, typeof(TSettings).Name);

        serviceCollection.AddSingleton(implementationInstance: _settings);
    }
}