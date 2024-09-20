using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.Memory;
using Expenso.Shared.Database.EfCore.Npsql;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Persistence.EfCore;
using Expenso.UserPreferences.Core.Persistence.EfCore.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Core;

public static class Extensions
{
    public static void AddUserPreferencesCore(this IServiceCollection services, IConfiguration configuration,
        string moduleName)
    {
        services.AddDbMigrator();

        configuration.TryBindOptions(sectionName: SectionNames.EfCoreSection,
            options: out EfCoreSettings databaseSettings);

        if (databaseSettings.InMemory is true)
        {
            services.AddMemoryDatabase<UserPreferencesDbContext>(moduleName: moduleName);
        }
        else
        {
            services.AddPostgres<UserPreferencesDbContext>(databaseSettings: databaseSettings);
        }

        services.AddScoped<IUserPreferencesDbContext, UserPreferencesDbContext>(implementationFactory: x =>
            x.GetRequiredService<UserPreferencesDbContext>());

        services.AddScoped<IPreferencesRepository, PreferencesRepository>();
    }
}