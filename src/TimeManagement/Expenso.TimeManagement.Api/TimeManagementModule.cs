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
        EndpointRegistration cancelJobEndpointRegistration = new(pattern: "cancel-job", name: "CancelJob",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Post, handler: async (
                [FromServices] ICommandHandler<CancelJobEntryCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] CancelJobEntryRequest model,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new CancelJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        CancelJobEntryRequest: model), cancellationToken: cancellationToken);

                return Results.NoContent();
            },
            description:
            "Cancels a job entry. The job entry to cancel is provided in the request body, and upon success, no content is returned.",
            summary: "Cancel a job entry", responses:
            [
                new Produces(StatusCode: StatusCodes.Status204NoContent, ContentType: typeof(void))
            ]);

        EndpointRegistration registerJobEndpointRegistration = new(pattern: "register-job", name: "RegisterJob",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Post, handler: async (
                [FromServices] ICommandHandler<RegisterJobEntryCommand, RegisterJobEntryResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] RegisterJobEntryRequest model,
                CancellationToken cancellationToken = default) =>
            {
                RegisterJobEntryResponse? response = await handler.HandleAsync(
                    command: new RegisterJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        RegisterJobEntryRequest: model), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            },
            description:
            "Registers a new job entry. The details of the job entry are provided in the request body, and upon success, the newly registered job entry information is returned.",
            summary: "Register a job entry", responses:
            [
                new Produces(StatusCode: StatusCodes.Status200OK, ContentType: typeof(RegisterJobEntryResponse))
            ]);

        return
        [
            cancelJobEndpointRegistration,
            registerJobEndpointRegistration
        ];
    }
}