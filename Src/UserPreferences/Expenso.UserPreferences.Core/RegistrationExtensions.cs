using Expenso.Shared.Database.EfCore.NpSql;
using Expenso.UserPreferences.Core.Data.Ef;
using Expenso.UserPreferences.Core.Data.Ef.Repositories;
using Expenso.UserPreferences.Core.Repositories;
using Expenso.UserPreferences.Core.Services;
using Expenso.UserPreferences.Core.Validators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Core;

public static class RegistrationExtensions
{
    public static void AddUserPreferences(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres<UserPreferencesDbContext>(configuration, "EfCore");

        services.AddScoped<IUserPreferencesDbContext, UserPreferencesDbContext>(x =>
            x.GetRequiredService<UserPreferencesDbContext>());

        services.AddScoped<IPreferencesRepository, PreferencesRepository>();
        services.AddScoped<IPreferencesService, PreferencesService>();
        services.AddScoped<IPreferenceValidator, PreferenceValidator>();
    }
}