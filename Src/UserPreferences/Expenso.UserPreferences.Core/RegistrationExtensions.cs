using Expenso.Shared.Database.EfCore.NpSql;
using Expenso.UserPreferences.Core.Application.Proxy;
using Expenso.UserPreferences.Core.Application.Services;
using Expenso.UserPreferences.Core.Application.Validators;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Persistence.EfCore;
using Expenso.UserPreferences.Core.Persistence.EfCore.Repositories;
using Expenso.UserPreferences.Proxy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Core;

public static class RegistrationExtensions
{
    public static void AddUserPreferencesModulesDependencies(this IServiceCollection services,
        IConfiguration configuration, string moduleName)
    {
        services.AddPostgres<UserPreferencesDbContext>(configuration, moduleName);

        services.AddScoped<IUserPreferencesDbContext, UserPreferencesDbContext>(x =>
            x.GetRequiredService<UserPreferencesDbContext>());

        services.AddScoped<IPreferencesRepository, PreferencesRepository>();
        services.AddScoped<IPreferencesService, PreferencesService>();
        services.AddScoped<IUserPreferencesProxy, UserPreferencesProxy>();
        services.AddScoped<IPreferenceValidator, PreferenceValidator>();
    }
}