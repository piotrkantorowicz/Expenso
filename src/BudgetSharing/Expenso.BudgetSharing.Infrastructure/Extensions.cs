using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Transactions;
using Expenso.Shared.Database;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.Memory;
using Expenso.Shared.Database.EfCore.Npsql;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        string moduleName)
    {
        services.AddDbMigrator();

        configuration.TryBindOptions(sectionName: SectionNames.EfCore,
            options: out EfCoreSettings databaseSettings);

        if (databaseSettings.InMemory is true)
        {
            services.AddMemoryDatabase<BudgetSharingDbContext>(moduleName: moduleName);
        }
        else
        {
            services.AddPostgres<BudgetSharingDbContext>(databaseSettings: databaseSettings);
        }

        services.AddScoped<IBudgetSharingDbContext, BudgetSharingDbContext>(implementationFactory: x =>
            x.GetRequiredService<BudgetSharingDbContext>());

        services.AddScoped<IBudgetPermissionRequestRepository, BudgetPermissionRequestRepository>();
        services.AddScoped<IBudgetPermissionRepository, BudgetPermissionRepository>();
        services.AddScoped<IBudgetPermissionRequestQueryStore, BudgetPermissionRequestQueryStore>();
        services.AddScoped<IBudgetPermissionQueryStore, BudgetPermissionQueryStore>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}