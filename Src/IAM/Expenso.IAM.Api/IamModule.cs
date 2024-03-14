﻿using System.Reflection;

using Expenso.IAM.Core;
using Expenso.IAM.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Api;

public sealed class IamModule : ModuleDefinition
{
    public override string ModulePrefix => "/users";

    public override Assembly[] GetModuleAssemblies()
    {
        return
        [
            typeof(IamModule).Assembly,
            typeof(Extensions).Assembly,
            typeof(IIamProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIamCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}