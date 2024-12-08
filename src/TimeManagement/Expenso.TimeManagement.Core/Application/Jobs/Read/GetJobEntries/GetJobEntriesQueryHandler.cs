using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Maps;
using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries;

internal sealed class
    GetJobEntriesQueryHandler : IQueryHandler<GetJobEntriesQuery, IReadOnlyCollection<GetJobEntriesResponse>>
{
    private readonly IJobEntryRepository _jobEntryRepository;

    public GetJobEntriesQueryHandler(IJobEntryRepository jobEntryRepository)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));
    }

    public async Task<IReadOnlyCollection<GetJobEntriesResponse>?> HandleAsync(GetJobEntriesQuery query,
        CancellationToken cancellationToken)
    {
        JobEntryQuerySpecification querySpecification = new()
        {
            JobEntryId = query.Payload?.JobEntryId,
            JobInstanceId = query.Payload?.JobInstanceId,
            JobEntryStatusIds = query.Payload?.JobEntryStatusIds,
            MoreThanRetries = query.Payload?.MoreThanRetries,
            IsCompleted = query.Payload?.IsCompleted,
            HasRunned = query.Payload?.HasRunned,
            IsActive = query.Payload?.IsActive,
            HasTriggers = query.Payload?.HasTriggers,
            Includes = query.Payload?.Includes.SafeCast<JobEntryIncludes, GetJobEntriesRequestJobEntryIncludes>()
        };

        IReadOnlyCollection<JobEntry> jobEntries =
            await _jobEntryRepository.GetJobEntries(querySpecification: querySpecification,
                cancellationToken: cancellationToken);

        return GetJobEntriesResponseMap.MapTo(jobEntries: jobEntries);
    }
}