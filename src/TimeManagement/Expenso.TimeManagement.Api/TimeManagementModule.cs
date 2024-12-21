using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Pagination;
using Expenso.TimeManagement.Core;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntry;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;
using Expenso.TimeManagement.Shared;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Extensions = Expenso.TimeManagement.Core.Extensions;

namespace Expenso.TimeManagement.Api;

public sealed class TimeManagementModule : IModuleDefinition
{
    public string ModuleName => ModuleNames.TimeManagementModule;

    public string ModulePrefix => "/time-management";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(TimeManagementModule).Assembly,
            typeof(Extensions).Assembly,
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
        EndpointRegistration getJobEntryEndpointRegistration = new(Pattern: "job-entries/{id}", Name: "GetJobEntry",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetJobEntryQuery, GetJobEntryResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromQuery] GetJobEntryRequestJobEntryIncludes? includes = null,
                CancellationToken cancellationToken = default) =>
            {
                GetJobEntryResponse? response = await handler.HandleAsync(
                    query: new GetJobEntryQuery(MessageContext: messageContextFactory.Current(),
                        Payload: new GetJobEntryRequest(JobEntryId: id, Includes: includes)),
                    cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        EndpointRegistration getJobEntriesEndpointRegistration = new(Pattern: "job-entries", Name: "GetJobEntries",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetJobEntriesQuery, IPagedList<GetJobEntriesResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? jobEntryId = null,
                [FromQuery] Guid? jobInstanceId = null, [FromQuery] Guid[]? jobEntryStatusIds = null,
                [FromQuery] int? moreThanRetries = null, [FromQuery] bool? isCompleted = null,
                [FromQuery] bool? hasRun = null, [FromQuery] bool? isActive = null,
                [FromQuery] bool? hasTriggers = null, [FromQuery] GetJobEntriesRequestJobEntryIncludes? includes = null,
                [FromQuery] Paging? pagination = null, CancellationToken cancellationToken = default) =>
            {
                IPagedList<GetJobEntriesResponse>? response = await handler.HandleAsync(
                    query: new GetJobEntriesQuery(MessageContext: messageContextFactory.Current(),
                        Pagination: pagination ?? Paging.Default,
                        Payload: new GetJobEntriesRequest(JobEntryId: jobEntryId, JobInstanceId: jobInstanceId,
                            JobEntryStatusIds: jobEntryStatusIds, MoreThanRetries: moreThanRetries,
                            IsCompleted: isCompleted, HasRun: hasRun, IsActive: isActive, HasTriggers: hasTriggers,
                            Includes: includes)), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        EndpointRegistration registerJobEntryEndpointRegistration = new(Pattern: "job-entries",
            Name: "RegisterJobEntry", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<RegisterJobEntryCommand, RegisterJobEntryResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] RegisterJobEntryRequest model,
                CancellationToken cancellationToken = default) =>
            {
                RegisterJobEntryResponse response = await handler.HandleAsync(
                    command: new RegisterJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        Payload: model), cancellationToken: cancellationToken);

                return Results.CreatedAtRoute(routeName: getJobEntryEndpointRegistration.Name, routeValues: new
                {
                    id = response.JobEntryId
                }, value: response);
            });

        EndpointRegistration cancelJobEntryEndpointRegistration = new(Pattern: "job-entries/{id}",
            Name: "CancelJobEntry", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Delete, Handler: async (
                [FromServices] ICommandHandler<CancelJobEntryCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new CancelJobEntryCommand(MessageContext: messageContextFactory.Current(),
                        Payload: new CancelJobEntryRequest(JobEntryId: id)), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        return
        [
            getJobEntryEndpointRegistration, getJobEntriesEndpointRegistration, cancelJobEntryEndpointRegistration,
            registerJobEntryEndpointRegistration
        ];
    }
}