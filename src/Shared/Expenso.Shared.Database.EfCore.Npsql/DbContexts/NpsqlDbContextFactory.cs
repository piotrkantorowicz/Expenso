using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Expenso.Shared.Database.EfCore.Npsql.DbContexts;

public abstract class NpsqlDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private const string SettingsFileName = "appsettings";

    public TDbContext CreateDbContext(string[]? args)
    {
        DbContextOptionsBuilder<TDbContext> optionsBuilder = new();
        string? environment = Environment.GetEnvironmentVariable(variable: "ASPNETCORE_ENVIRONMENT");
        string? startupProjectPath = args?[0];

        if (string.IsNullOrEmpty(value: startupProjectPath) || !Path.Exists(path: startupProjectPath))
        {
            string errorMessage =
                $"Startup project path parameter must be provided and must exists on current machine. Actual value: {startupProjectPath}.";

            throw new ArgumentException(message: errorMessage);
        }

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath: startupProjectPath)
            .AddJsonFile(path: $"{SettingsFileName}.json", optional: false, reloadOnChange: true)
            .AddJsonFile(path: $"{SettingsFileName}.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        configuration.TryBindOptions(sectionName: SectionNames.EfCoreSection,
            options: out EfCoreSettings databaseSettings);

        optionsBuilder.UseNpgsql(connectionString: databaseSettings.ConnectionParameters?.ConnectionString);

        return (TDbContext)Activator.CreateInstance(type: typeof(TDbContext), args: [optionsBuilder.Options])!;
    }
}