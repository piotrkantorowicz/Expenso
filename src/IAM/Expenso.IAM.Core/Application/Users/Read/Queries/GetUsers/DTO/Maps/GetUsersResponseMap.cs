using Expenso.IAM.Shared.DTO.GetUsers.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;

public static class GetUsersResponseMap
{
    public static IReadOnlyCollection<GetUsersResponse> MapTo(IEnumerable<UserRepresentation> user)
    {
        return new List<GetUsersResponse>(collection: user.Select(selector: MapTo)).AsReadOnly();
    }

    private static GetUsersResponse MapTo(UserRepresentation user)
    {
        return new GetUsersResponse(UserId: user.Id, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username, Email: user.Email);
    }
}