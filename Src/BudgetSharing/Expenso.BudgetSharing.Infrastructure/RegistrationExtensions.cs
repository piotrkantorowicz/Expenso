using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories;
using Expenso.Shared.Database.EfCore.NpSql;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Infrastructure;

public static class RegistrationExtensions
{
    public static void AddBudgetSharingModulesDependencies(this IServiceCollection services,
        IConfiguration configuration, string moduleName)
    {
        services.AddPostgres<BudgetSharingDbContext>(configuration, moduleName);

        services.AddScoped<IBudgetSharingDbContext, BudgetSharingDbContext>(x =>
            x.GetRequiredService<BudgetSharingDbContext>());

        services.AddScoped<IBudgetPermissionRequestRepository, BudgetPermissionRequestRepository>();
        services.AddScoped<IBudgetPermissionRepository, BudgetPermissionRepository>();
    }
}