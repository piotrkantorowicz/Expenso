using Expenso.IAM.Shared.DTO.GetUserById.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;

public static class GetUserByIdResponseMap
{
    public static GetUserByIdResponse MapTo(UserRepresentation user)
    {
        return new GetUserByIdResponse(UserId: user.Id!, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username!, Email: user.Email!);
    }
}