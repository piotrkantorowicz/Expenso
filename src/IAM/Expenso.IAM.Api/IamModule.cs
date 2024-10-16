﻿using System.Reflection;

using Expenso.IAM.Core;
using Expenso.IAM.Shared;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Extensions = Expenso.IAM.Core.Extensions;

namespace Expenso.IAM.Api;

public sealed class IamModule : IModuleDefinition
{
    public string ModulePrefix => "/users";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(IamModule).Assembly,
            typeof(Extensions).Assembly,
            typeof(IIamProxy).Assembly
        ];
    }

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIamCore(configuration: configuration);
        services.AddIamProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}