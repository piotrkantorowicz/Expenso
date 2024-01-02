using Expenso.IAM.Core.Services.Interfaces;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Core.Proxy;

internal sealed class IamProxy(IUserService userService) : IIamProxy
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<UserContract?> GetUserByIdAsync(string userId)
    {
        return await _userService.GetUserByIdInternalAsync(userId);
    }

    public async Task<UserContract?> GetUserByEmailAsync(string email)
    {
        return await _userService.GetUserByEmailInternalAsync(email);
    }
}