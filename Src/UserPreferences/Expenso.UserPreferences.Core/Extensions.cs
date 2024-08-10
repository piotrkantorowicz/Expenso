using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.Memory;
using Expenso.Shared.Database.EfCore.NpSql;
using Expenso.Shared.System.Configuration.Extensions;
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
        configuration.TryBindOptions(SectionNames.EfCoreSection, out EfCoreSettings databaseSettings);

        if (databaseSettings.InMemory is true)
            services.AddMemoryDatabase<UserPreferencesDbContext>(moduleName);
        else
            services.AddPostgres<UserPreferencesDbContext>(databaseSettings);

        services.AddScoped<IUserPreferencesDbContext, UserPreferencesDbContext>(x =>
            x.GetRequiredService<UserPreferencesDbContext>());

        services.AddScoped<IPreferencesRepository, PreferencesRepository>();
    }
}