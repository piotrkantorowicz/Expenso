using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;

namespace Expenso.IAM.Shared;

public interface IIamProxy
{
    Task<GetUserByIdResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserByEmailResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}