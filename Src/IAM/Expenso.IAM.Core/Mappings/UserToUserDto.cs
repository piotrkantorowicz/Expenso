using Expenso.IAM.Proxy.DTO;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Mappings;

internal static class UserToUserDto
{
    public static UserDto? Map(User? user)
    {
        return user is null ? null : new UserDto(user.Id!, user.FirstName, user.LastName, user.Username!, user.Email!);
    }
}