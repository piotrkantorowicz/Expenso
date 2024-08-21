using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain;

public static class Extensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAssignParticipantionDomainService, AssignParticipantionDomainService>();
        services.AddScoped<IConfirmParticipantionDomainService, ConfirmParticipantionDomainService>();

        services
            .AddScoped<IBudgetPermissionRequestExpirationDomainService,
                BudgetPermissionRequestExpirationDomainService>();
    }
}