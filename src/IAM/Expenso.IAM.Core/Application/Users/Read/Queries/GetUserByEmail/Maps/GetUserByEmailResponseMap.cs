using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.Maps;

public static class GetUserByEmailResponseMap
{
    public static GetUserByEmailResponse MapTo(UserRepresentation user)
    {
        return new GetUserByEmailResponse(UserId: user.Id!, Firstname: user.FirstName, Lastname: user.LastName,
            Username: user.Username!, Email: user.Email!);
    }
}