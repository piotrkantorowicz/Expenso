using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy;
using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreExtensions = Expenso.TimeManagement.Core.Extensions;

namespace Expenso.TimeManagement.Api;

public sealed class TimeManagementModule : IModuleDefinition
{
    public string ModulePrefix => "/time-management";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(TimeManagementModule).Assembly,
            typeof(CoreExtensions).Assembly,
            typeof(ITimeManagementProxy).Assembly
        ];
    }

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTimeManagementCore(configuration: configuration, moduleName: GetType().Name);
        services.AddTimeManagementProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        EndpointRegistration cancelJobEndpointRegistration = new(Pattern: "cancel-job", Name: "CancelJob",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<CancelJobEntryCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] CancelJobEntryRequest model,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new CancelJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        CancelJobEntryRequest: model), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration registerJobEndpointRegistration = new(Pattern: "register-job", Name: "RegisterJob",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<RegisterJobEntryCommand, RegisterJobEntryResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] RegisterJobEntryRequest model,
                CancellationToken cancellationToken = default) =>
            {
                RegisterJobEntryResponse? response = await handler.HandleAsync(
                    command: new RegisterJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        RegisterJobEntryRequest: model), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        return [cancelJobEndpointRegistration, registerJobEndpointRegistration];
    }
}