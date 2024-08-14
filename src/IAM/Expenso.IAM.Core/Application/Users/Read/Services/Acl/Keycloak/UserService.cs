using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser.DTO.Response.Maps;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Sdk;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;

internal sealed class UserService : IUserService
{
    private readonly KeycloakProtectionClientOptions _keycloakProtectionClientOptions;
    private readonly IKeycloakUserClient _keycloakUserClient;

    public UserService(IKeycloakUserClient keycloakUserClient,
        KeycloakProtectionClientOptions keycloakProtectionClientOptions)
    {
        _keycloakProtectionClientOptions = keycloakProtectionClientOptions ??
                                           throw new ArgumentNullException(
                                               paramName: nameof(keycloakProtectionClientOptions));

        _keycloakUserClient =
            keycloakUserClient ?? throw new ArgumentNullException(paramName: nameof(keycloakUserClient));
    }

    public async Task<GetUserResponse> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        UserRepresentation keycloakUser = await _keycloakUserClient.GetUserAsync(
            realm: _keycloakProtectionClientOptions.Realm, userId: userId, cancellationToken: cancellationToken);

        if (keycloakUser is null)
        {
            throw new NotFoundException(message: $"User with id {userId} not found");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(user: keycloakUser);

        return getUserResponse;
    }

    public async Task<GetUserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        List<UserRepresentation> keycloakUsers = (await _keycloakUserClient.GetUsersAsync(
            realm: _keycloakProtectionClientOptions.Realm, parameters: new GetUsersRequestParameters
            {
                Email = email
            }, cancellationToken: cancellationToken)).ToList();

        UserRepresentation? user = keycloakUsers.Count == 0 ? null : keycloakUsers.Single();

        if (user is null)
        {
            throw new NotFoundException(message: $"User with email {email} not found");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(user: user);

        return getUserResponse;
    }
}