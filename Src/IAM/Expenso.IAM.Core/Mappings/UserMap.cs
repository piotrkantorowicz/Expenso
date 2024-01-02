using Expenso.IAM.Core.DTO;
using Expenso.IAM.Proxy.Contracts;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Mappings;

internal static class UserMap
{
    public static UserDto? MapToDto(User? user)
    {
        return user is null ? null : new UserDto(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }

    public static UserContract? MapToContract(User? user)
    {
        return user is null
            ? null
            : new UserContract(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }
}