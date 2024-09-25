using Expenso.Api.Configuration.Settings.Exceptions;

namespace Expenso.Api.Configuration.Extensions;

internal static class AppConfigurationManagerExtensions
{
    public static T GetRequiredSettings<T>(this AppConfigurationManager? appConfigurationManager, string sectionName)
        where T : class
    {
        return appConfigurationManager?.GetSettings<T>(sectionName: sectionName) ??
               throw new ConfigurationHasNotBeenInitializedYetException();
    }
}