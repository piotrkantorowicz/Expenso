using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.Memory;
using Expenso.Shared.Database.EfCore.NpSql;
using Expenso.Shared.System.Configuration.Extensions;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers.Interfaces;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Core.Persistence.EfCore;
using Expenso.TimeManagement.Core.Persistence.EfCore.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.TimeManagement.Core;

public static class Extensions
{
    public static void AddTimeManagementCore(this IServiceCollection services, IConfiguration configuration,
        string moduleName)
    {
        services.AddDbMigrator();
        configuration.TryBindOptions(SectionNames.EfCoreSection, out EfCoreSettings databaseSettings);

        if (databaseSettings.InMemory is true)
            services.AddMemoryDatabase<TimeManagementDbContext>(moduleName);
        else
            services.AddPostgres<TimeManagementDbContext>(databaseSettings);

        services.AddScoped<ITimeManagementDbContext, TimeManagementDbContext>(x =>
            x.GetRequiredService<TimeManagementDbContext>());

        services.AddScoped<IJobEntryRepository, JobEntryRepository>();
        services.AddScoped<IJobEntryStatusRepository, JobEntryStatusRepository>();
        services.AddScoped<IJobEntryTypeRepository, JobEntryTypeRepository>();
        services.AddScoped<IJobEntryPeriodIntervalHelper, JobEntryPeriodIntervalHelper>();
        services.AddHostedService<BudgetSharingRequestsExpirationJob>();
    }
}