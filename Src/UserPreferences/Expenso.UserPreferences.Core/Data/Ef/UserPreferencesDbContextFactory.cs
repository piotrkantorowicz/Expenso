using Expenso.Shared.Configuration.Extensions;
using Expenso.Shared.Configuration.Sections;
using Expenso.Shared.Database.EfCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Expenso.UserPreferences.Core.Data.Ef;

internal sealed class UserPreferencesDbContextFactory : IDesignTimeDbContextFactory<UserPreferencesDbContext>
{
    private const string SettingsFileName = "appsettings";

    public UserPreferencesDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<UserPreferencesDbContext> optionsBuilder = new();
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string rootPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
        string startupProjectPath = Path.Combine(rootPath, "Src", "Api", "Expenso.Api");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(startupProjectPath)
            .AddJsonFile($"{SettingsFileName}.json", false, true)
            .AddJsonFile($"{SettingsFileName}.{environment}.json", true)
            .AddEnvironmentVariables()
            .Build();

        configuration.TryBindOptions(SectionNames.EfCoreSection, out EfCoreSettings databaseSettings);
        optionsBuilder.UseNpgsql(databaseSettings.ConnectionString);

        return new UserPreferencesDbContext(optionsBuilder.Options);
    }
}