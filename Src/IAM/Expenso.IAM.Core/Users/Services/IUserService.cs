using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Core.Users.Services;

public interface IUserService
{
    Task<GetUserResponse> GetUserByIdAsync(string userId);

    Task<GetUserResponse> GetUserByEmailAsync(string email);
}