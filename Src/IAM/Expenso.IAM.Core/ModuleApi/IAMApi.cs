using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Core.Services.Interfaces;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Core.ModuleApi;

internal sealed class IamApi(IUserService userService) : IIamApi
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<UserContract?> GetUserById(string userId)
    {
        UserDto? userDto = await _userService.GetUserById(userId);

        return UserDtoToUserContract.Map(userDto);
    }

    public async Task<UserContract?> GetUserByEmail(string email)
    {
        UserDto? userDto = await _userService.GetUserByEmail(email);

        return UserDtoToUserContract.Map(userDto);
    }
}