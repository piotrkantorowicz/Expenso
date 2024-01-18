using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Core.Users.Services;

public interface IUserService
{
    Task<GetUserResponse> GetUserByIdAsync(string userId);

    Task<GetUserInternalResponse> GetUserByIdInternalAsync(string userId);

    Task<GetUserResponse> GetUserByEmailAsync(string email);

    Task<GetUserInternalResponse> GetUserByEmailInternalAsync(string email);
}