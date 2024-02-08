using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Domain;

public static class RegistrationExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IAssignParticipantDomainService, AssignParticipantDomainService>();
    }
}