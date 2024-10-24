﻿using Expenso.Api.Configuration.Settings.Exceptions;
using Expenso.Api.Configuration.Settings.Services.Containers;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Logging;

namespace Expenso.Api.Configuration;

internal sealed class AppConfigurationManager : IAppConfigurationManager
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

        IReadOnlyCollection<ISettingsBinder?> binders = _preStartupContainer.ResolveMany<ISettingsBinder>();

        if (binders.Count is 0)
        {
            logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                message: "No ISettingsBinder services were found");

            return;
        }

        foreach (ISettingsBinder? binder in binders)
        {
            if (binder is null)
            {
                logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "A null binder was encountered and skipped");

                continue;
            }

            string sectionName = binder.GetSectionName();
            object? boundSettings = binder.Bind(serviceCollection: serviceCollection);

            if (boundSettings is not null)
            {
                _settingsMap[key: sectionName] = boundSettings;

                logger?.LogInfo(eventId: LoggingUtils.ConfigurationInformation,
                    message: "Settings of type {SettingsType} have been successfully added into settings map",
                    messageContext: null, boundSettings.GetType().Name);
            }
            else
            {
                logger?.LogWarning(eventId: LoggingUtils.ConfigurationWarning,
                    message: "Failed to add settings from section {SectionName} into settings map", exception: null,
                    messageContext: null, sectionName);
            }
        }

        _configured = true;
        logger?.LogInfo(eventId: LoggingUtils.ConfigurationInformation, message: "Configuration of settings completed");
    }
}