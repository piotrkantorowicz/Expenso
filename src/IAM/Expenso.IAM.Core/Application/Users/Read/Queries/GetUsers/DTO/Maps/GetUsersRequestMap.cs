using Expenso.IAM.Shared.DTO.GetUsers.Request;

using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;

public static class GetUsersRequestMap
{
    public static GetUsersRequestParameters MapTo(GetUsersRequest? request)
    {
        return new GetUsersRequestParameters
        {
            IdpUserId = request?.UserId,
            FirstName = request?.Firstname,
            LastName = request?.Lastname,
            Email = request?.Email,
            Username = request?.Username,
            Max = request?.Limit,
            Exact = request?.Exact
        };
    }
}