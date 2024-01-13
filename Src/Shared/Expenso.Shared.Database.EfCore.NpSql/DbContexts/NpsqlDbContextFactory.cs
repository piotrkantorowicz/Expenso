using System.Text;

using Expenso.Shared.Configuration.Extensions;
using Expenso.Shared.Configuration.Sections;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Expenso.Shared.Database.EfCore.NpSql.DbContexts;

public abstract class NpsqlDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private const string SettingsFileName = "appsettings";

    public TDbContext CreateDbContext(string[]? args)
    {
        DbContextOptionsBuilder<TDbContext> optionsBuilder = new();
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string? startupProjectPath = args?[0];

        if (string.IsNullOrEmpty(startupProjectPath) || !Path.Exists(startupProjectPath))
        {
            string errorMessage = new StringBuilder()
                .Append(
                    "Startup project path parameter must be provided and must exists on current machine. Actual value: ")
                .Append(startupProjectPath)
                .ToString();

            throw new ArgumentException(errorMessage);
        }

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(startupProjectPath)
            .AddJsonFile($"{SettingsFileName}.json", false, true)
            .AddJsonFile($"{SettingsFileName}.{environment}.json", true)
            .AddEnvironmentVariables()
            .Build();

        configuration.TryBindOptions(SectionNames.EfCoreSection, out EfCoreSettings databaseSettings);
        optionsBuilder.UseNpgsql(databaseSettings.ConnectionString);

        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), [optionsBuilder.Options])!;
    }
}