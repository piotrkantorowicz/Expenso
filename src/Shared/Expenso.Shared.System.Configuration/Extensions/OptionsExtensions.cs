using Microsoft.Extensions.Configuration;

namespace Expenso.Shared.System.Configuration.Extensions;

public static class OptionsExtensions
{
    public static bool TryBindOptions<T>(this IConfiguration configuration, string sectionName, out T options,
        bool bindNonPublicProperties = true) where T : new()
    {
        options = new T();

        try
        {
            configuration
                .GetSection(key: sectionName)
                .Bind(instance: options, configureOptions: opt =>
                {
                    if (bindNonPublicProperties)
                    {
                        opt.BindNonPublicProperties = true;
                    }
                });
        }
        catch
        {
            return false;
        }

        return true;
    }
}