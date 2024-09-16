using System.Reflection;

using Expenso.Api.Configuration.Settings.Exceptions;
using Expenso.Api.Configuration.Settings.Services;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Configuration;

internal sealed class AppConfigurationManager
{
    private readonly IPreStartupContainer _preStartupContainer;
    private readonly Dictionary<string, object?> _settingsMap = new();
    private bool _configured;

    public AppConfigurationManager(IPreStartupContainer preStartupContainer)
    {
        _preStartupContainer = preStartupContainer ??
                               throw new ArgumentNullException(paramName: nameof(preStartupContainer));
    }

    public TSettings GetSettings<TSettings>(string sectionName) where TSettings : class
    {
        ILoggerService<AppConfigurationManager>? logger =
            _preStartupContainer.Resolve<ILoggerService<AppConfigurationManager>>();

        if (!_configured)
        {
            logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "Configuration has not been initialized yet");

            throw new ConfigurationHasNotBeenInitializedYetException();
        }

        if (_settingsMap.TryGetValue(key: sectionName, value: out object? settingsObj))
        {
            if (settingsObj is not TSettings settings)
            {
                throw new InvalidSettingsTypeException();
            }

            return settings;
        }

        logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
            message: "Settings not found for section: {SectionName}", exception: null, messageContext: null,
            sectionName);

        throw new MissingSettingsSectionException(sectionName: sectionName);
    }

    public void Configure(IServiceCollection serviceCollection)
    {
        ILoggerService<AppConfigurationManager>? logger =
            _preStartupContainer.Resolve<ILoggerService<AppConfigurationManager>>();

        logger?.LogInfo(eventId: LoggingUtils.ConfigurationInformation, message: "Starting configuration of settings");

        if (_configured)
        {
            logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "Settings have already been configured. Exiting configuration");

            return;
        }

        IReadOnlyCollection<object?> binders = _preStartupContainer.ResolveMany<ISettingsBinder>();

        if (binders.Count == 0)
        {
            logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "No ISettingsBinder services were found");

            return;
        }

        foreach (object? binder in binders)
        {
            if (binder is null)
            {
                logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "A null binder was encountered and skipped");

                continue;
            }

            Type binderType = binder.GetType();
            MethodInfo? getSectionNameMethod = binderType.GetMethod(name: "GetSectionName");
            MethodInfo? bindMethod = binderType.GetMethod(name: "Bind");

            if (getSectionNameMethod == null || bindMethod == null)
            {
                logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "Binder of type {BinderType} does not have the expected methods", exception: null,
                    messageContext: null, binderType.Name);

                continue;
            }

            string? sectionName = getSectionNameMethod.Invoke(obj: binder, parameters: null) as string;

            object? boundSettings = bindMethod.Invoke(obj: binder, parameters:
            [
                serviceCollection
            ]);

            if (sectionName != null)
            {
                _settingsMap.Add(key: sectionName, value: boundSettings);

                logger?.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
                    message: "Settings for section {SectionName} have been bound successfully", messageContext: null,
                    sectionName);
            }
            else
            {
                logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "Section name returned null for binder of type {BinderType}", exception: null,
                    messageContext: null, binderType.Name);
            }
        }

        logger?.LogInfo(eventId: LoggingUtils.ConfigurationInformation, message: "Configuration process completed.");
        _configured = true;
    }
}