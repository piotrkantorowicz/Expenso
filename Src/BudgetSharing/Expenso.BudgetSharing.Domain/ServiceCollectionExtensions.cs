using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAssignParticipantDomainService, AssignParticipantDomainService>();
        services.AddScoped<IConfirmParticipationDomainService, ConfirmParticipationDomainService>();
    }
}