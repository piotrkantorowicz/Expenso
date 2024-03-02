using Expenso.IAM.Proxy.DTO.GetUser;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response.Maps;

internal static class GetUserResponseMap
{
    public static GetUserResponse MapTo(User user)
    {
        return new GetUserResponse(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }
}