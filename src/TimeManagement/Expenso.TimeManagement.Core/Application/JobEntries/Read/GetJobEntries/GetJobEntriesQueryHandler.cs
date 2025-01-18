using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Maps;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries;

internal sealed class GetJobEntriesQueryHandler : IQueryHandler<GetJobEntriesQuery, IPagedList<GetJobEntriesResponse>>
{
    private readonly IJobEntryRepository _jobEntryRepository;

    public GetJobEntriesQueryHandler(IJobEntryRepository jobEntryRepository)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));
    }

    public async Task<IPagedList<GetJobEntriesResponse>?> HandleAsync(GetJobEntriesQuery query,
        CancellationToken cancellationToken)
    {
        JobEntryQuerySpecification querySpecification = new(JobEntryId: query.Payload?.JobEntryId,
            JobInstanceId: query.Payload?.JobInstanceId, JobEntryStatusIds: query.Payload?.JobEntryStatusIds,
            MoreThanRetries: query.Payload?.MoreThanRetries, IsCompleted: query.Payload?.IsCompleted,
            HasRun: query.Payload?.HasRun, IsActive: query.Payload?.IsActive, HasTriggers: query.Payload?.HasTriggers,
            Includes: query.Payload?.Includes.SafeCast<JobEntryIncludes, GetJobEntriesRequestJobEntryIncludes>());

        IPagedList<JobEntry> jobEntries = await _jobEntryRepository.GetJobEntriesAsync(
            querySpecification: querySpecification, pagination: DatabasePagination.New(pagination: query.Pagination),
            sorters: DatabaseSorting.New(sorting: query.Sorters), cancellationToken: cancellationToken);

        return GetJobEntriesResponseMap.MapTo(jobEntries: jobEntries);
    }
}