using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntry.DTO.Maps;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntry;

internal sealed class GetJobEntryQueryHandler : IQueryHandler<GetJobEntryQuery, GetJobEntryResponse>
{
    private readonly IJobEntryRepository _jobEntryRepository;

    public GetJobEntryQueryHandler(IJobEntryRepository jobEntryRepository)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));
    }

    public async Task<GetJobEntryResponse?> HandleAsync(GetJobEntryQuery query, CancellationToken cancellationToken)
    {
        JobEntryQuerySpecification querySpecification = new(JobEntryId: query.Payload?.JobEntryId,
            Includes: query.Payload?.Includes.SafeCast<JobEntryIncludes, GetJobEntryRequestJobEntryIncludes>(),
            UseTracking: false);

        JobEntry? jobEntry = await _jobEntryRepository.GetJobEntryAsync(querySpecification: querySpecification,
            cancellationToken: cancellationToken);

        if (jobEntry is null)
        {
            throw new NotFoundException(resourceName: nameof(JobEntry), identifierType: IdentifierType.PrimaryId(),
                identifier: query.Payload?.JobEntryId);
        }

        return GetJobEntryResponseMap.MapTo(jobEntry: jobEntry);
    }
}