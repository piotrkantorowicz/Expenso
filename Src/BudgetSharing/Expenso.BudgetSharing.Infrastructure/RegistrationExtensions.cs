using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Transactions;
using Expenso.Shared.Database;
using Expenso.Shared.Database.EfCore.NpSql;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Infrastructure;

public static class RegistrationExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        string moduleName)
    {
        services.AddPostgres<BudgetSharingDbContext>(configuration, moduleName);

        services.AddScoped<IBudgetSharingDbContext, BudgetSharingDbContext>(x =>
            x.GetRequiredService<BudgetSharingDbContext>());

        services.AddScoped<IBudgetPermissionRequestRepository, BudgetPermissionRequestRepository>();
        services.AddScoped<IBudgetPermissionRepository, BudgetPermissionRepository>();
        services.AddScoped<IBudgetPermissionRequestQueryStore, BudgetPermissionRequestQueryStore>();
        services.AddScoped<IBudgetPermissionQueryStore, BudgetPermissionQueryStore>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}