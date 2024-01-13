using Expenso.IAM.Core;
using Expenso.Shared.ModuleDefinition;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Api;

public sealed class IamModule : ModuleDefinition
{
    public override string ModulePrefix => "/users";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIamCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}