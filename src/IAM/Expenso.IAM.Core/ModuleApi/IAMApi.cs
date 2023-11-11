using Expenso.IAM.Core.Services.Interfaces;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO;

namespace Expenso.IAM.Core.ModuleApi;

internal sealed class IamApi(IUserService userService) : IIamApi
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<UserDto?> GetUserById(string userId)
    {
        return await _userService.GetUserById(userId);
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        return await _userService.GetUserByEmail(email);
    }
}