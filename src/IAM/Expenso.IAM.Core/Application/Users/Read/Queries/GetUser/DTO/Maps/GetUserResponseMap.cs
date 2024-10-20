using Expenso.IAM.Shared.DTO.GetUser.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUser.DTO.Maps;

internal static class GetUserResponseMap
{
    public static GetUserResponse MapTo(UserRepresentation user)
    {
        return new GetUserResponse(UserId: user.Id!, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username!, Email: user.Email!);
    }
}