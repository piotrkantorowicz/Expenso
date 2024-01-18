using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Mappings;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Users.Services.Acl.Keycloak;

internal sealed class UserService(
    IKeycloakUserClient keycloakUserClient,
    KeycloakProtectionClientOptions keycloakProtectionClientOptions) : IUserService
{
    private readonly KeycloakProtectionClientOptions _keycloakProtectionClientOptions =
        keycloakProtectionClientOptions ?? throw new ArgumentNullException(nameof(keycloakProtectionClientOptions));

    private readonly IKeycloakUserClient _keycloakUserClient =
        keycloakUserClient ?? throw new ArgumentNullException(nameof(keycloakUserClient));

    public async Task<GetUserResponse> GetUserByIdAsync(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);

        if (keycloakUser is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        GetUserResponse getUserResponse = UserMap.MapToDto(keycloakUser);

        return getUserResponse;
    }

    public async Task<GetUserInternalResponse> GetUserByIdInternalAsync(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);

        if (keycloakUser is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        GetUserInternalResponse getUserInternalResponse = UserMap.MapToContract(keycloakUser);

        return getUserInternalResponse;
    }

    public async Task<GetUserResponse> GetUserByEmailAsync(string email)
    {
        User? user = await GetByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        GetUserResponse getUserResponse = UserMap.MapToDto(user);

        return getUserResponse;
    }

    public async Task<GetUserInternalResponse> GetUserByEmailInternalAsync(string email)
    {
        User? user = await GetByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        GetUserInternalResponse getUserInternalResponse = UserMap.MapToContract(user);

        return getUserInternalResponse;
    }

    private async Task<User?> GetByEmailAsync(string email)
    {
        List<User> keycloakUsers = (await _keycloakUserClient.GetUsers(_keycloakProtectionClientOptions.Realm,
            new GetUsersRequestParameters
            {
                Email = email
            })).ToList();

        return keycloakUsers.Count == 0 ? null : keycloakUsers.Single();
    }
}