using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain;

public static class Extensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAssignParticipationDomainService, AssignParticipationDomainService>();
        services.AddScoped<IConfirmParticipantionDomainService, ConfirmParticipantionDomainService>();
        services.AddScoped<IIamProxyService, IamProxyService>();

        services
            .AddScoped<IBudgetPermissionRequestExpirationDomainService,
                BudgetPermissionRequestExpirationDomainService>();
    }
}