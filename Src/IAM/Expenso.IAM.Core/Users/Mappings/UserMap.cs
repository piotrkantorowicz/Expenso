using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Users.Mappings;

internal static class UserMap
{
    public static GetUserResponse MapToDto(User user)
    {
        return new GetUserResponse(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }

    public static GetUserInternalResponse MapToContract(User user)
    {
        return new GetUserInternalResponse(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }
}