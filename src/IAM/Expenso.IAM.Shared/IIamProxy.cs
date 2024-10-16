using Expenso.IAM.Shared.DTO.GetUser;

namespace Expenso.IAM.Shared;

public interface IIamProxy
{
    Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}