using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.IAM.Core.Application.Users.Read.Services;

public interface IUserService
{
    Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest? request, CancellationToken cancellationToken);

    Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest? request,
        CancellationToken cancellationToken);

    Task<IPagedList<GetUsersResponse>> GetUsersAsync(GetUsersRequest? request, Paging? pagination,
        CancellationToken cancellationToken);
}