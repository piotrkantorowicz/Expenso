using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser.DTO.Response.Maps;
using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;

internal sealed class UserService : IUserService
{
    private readonly KeycloakSettings _keycloakSettings;
    private readonly IKeycloakUserClient _keycloakUserClient;

    public UserService(IKeycloakUserClient keycloakUserClient, KeycloakSettings keycloakSettings)
    {
        _keycloakSettings = keycloakSettings ?? throw new ArgumentNullException(paramName: nameof(keycloakSettings));

        _keycloakUserClient =
            keycloakUserClient ?? throw new ArgumentNullException(paramName: nameof(keycloakUserClient));
    }

    public async Task<GetUserResponse> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        UserRepresentation keycloakUser = await _keycloakUserClient.GetUserAsync(realm: _keycloakSettings.Realm,
            userId: userId, cancellationToken: cancellationToken);

        if (keycloakUser is null)
        {
            throw new NotFoundException(message: $"User with ID {userId} not found");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(user: keycloakUser);

        return getUserResponse;
    }

    public async Task<GetUserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        List<UserRepresentation> keycloakUsers = (await _keycloakUserClient.GetUsersAsync(
            realm: _keycloakSettings.Realm, parameters: new GetUsersRequestParameters
            {
                Email = email
            }, cancellationToken: cancellationToken)).ToList();

        UserRepresentation? user = keycloakUsers.Count is 0 ? null : keycloakUsers.Single();

        if (user is null)
        {
            throw new NotFoundException(message: $"User with email {email} not found");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(user: user);

        return getUserResponse;
    }
}