using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.Shared.System.Types.Pagination;

using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;

public static class GetUsersRequestMap
{
    public static GetUsersRequestParameters MapTo(GetUsersRequest? request, Paging? pagination)
    {
        return new GetUsersRequestParameters
        {
            IdpUserId = request?.UserId,
            FirstName = request?.Firstname,
            LastName = request?.Lastname,
            Email = request?.Email,
            Username = request?.Username,
            Exact = request?.Exact,
            Max = pagination?.Limit
        };
    }
}