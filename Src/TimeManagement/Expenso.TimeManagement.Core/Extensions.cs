using Expenso.Shared.Database.EfCore.NpSql;
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
        services.AddPostgres<TimeManagementDbContext>(configuration, moduleName);

        services.AddScoped<ITimeManagementDbContext, TimeManagementDbContext>(x =>
            x.GetRequiredService<TimeManagementDbContext>());

        services.AddScoped<IJobEntryRepository, JobEntryRepository>();
        services.AddScoped<IJobEntryStatusRepository, JobEntryStatusRepository>();
        services.AddScoped<IJobEntryTypeRepository, JobEntryTypeRepository>();
        services.AddScoped<IJobEntryPeriodIntervalHelper, JobEntryPeriodIntervalHelper>();
        services.AddHostedService<BudgetSharingRequestsExpirationJob>();
    }
}