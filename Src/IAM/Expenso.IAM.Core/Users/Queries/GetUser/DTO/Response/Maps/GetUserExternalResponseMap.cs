using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response.Maps;

internal static class GetUserExternalResponseMap
{
    public static GetUserExternalResponse? MapTo(GetUserResponse? userResponse)
    {
        return userResponse is null
            ? null
            : new GetUserExternalResponse(userResponse.UserId, userResponse.Firstname, userResponse.Lastname,
                userResponse.Username, userResponse.Email);
    }
}