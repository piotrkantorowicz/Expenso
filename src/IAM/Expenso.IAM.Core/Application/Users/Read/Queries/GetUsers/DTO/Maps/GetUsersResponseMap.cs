using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Collections;
using Expenso.Shared.System.Types.Paging;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;

public static class GetUsersResponseMap
{
    public static IPagedList<GetUsersResponse> MapTo(IEnumerable<UserRepresentation> users, Pagination? pagination)
    {
        return users.Pagination(pagination: pagination).Map(map: MapTo);
    }

    public static GetUsersResponse MapTo(UserRepresentation user)
    {
        return new GetUsersResponse(UserId: user.Id, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username, Email: user.Email);
    }
}