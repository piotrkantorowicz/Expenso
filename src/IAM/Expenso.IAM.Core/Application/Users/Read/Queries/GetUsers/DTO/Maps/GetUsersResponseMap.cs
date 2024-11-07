using Expenso.IAM.Shared.DTO.GetUsers.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;

public static class GetUsersResponseMap
{
    public static IReadOnlyCollection<GetUsersResponse> MapTo(IEnumerable<UserRepresentation> users)
    {
        return new List<GetUsersResponse>(collection: users.Select(selector: MapTo)).AsReadOnly();
    }

    public static GetUsersResponse MapTo(UserRepresentation user)
    {
        return new GetUsersResponse(UserId: user.Id, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username, Email: user.Email);
    }
}