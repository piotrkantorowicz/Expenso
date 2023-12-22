using Expenso.IAM.Core.DTO;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Core.Mappings;

internal static class UserDtoToUserContract
{
    public static UserContract? Map(UserDto? userDto)
    {
        return userDto is null
            ? null
            : new UserContract(userDto.UserId, userDto.Firstname, userDto.Lastname, userDto.Username, userDto.Email);
    }
}