using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Services;

internal sealed class UserService(
    IKeycloakUserClient keycloakUserClient,
    KeycloakProtectionClientOptions keycloakProtectionClientOptions) : IUserService
{
    private readonly KeycloakProtectionClientOptions _keycloakProtectionClientOptions =
        keycloakProtectionClientOptions ?? throw new ArgumentNullException(nameof(keycloakProtectionClientOptions));

    private readonly IKeycloakUserClient _keycloakUserClient =
        keycloakUserClient ?? throw new ArgumentNullException(nameof(keycloakUserClient));

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);

        if (keycloakUser is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        UserDto userDto = UserMap.MapToDto(keycloakUser);

        return userDto;
    }

    public async Task<UserContract> GetUserByIdInternalAsync(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);

        if (keycloakUser is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        UserContract userContract = UserMap.MapToContract(keycloakUser);

        return userContract;
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        User? user = await GetByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        UserDto userDto = UserMap.MapToDto(user);

        return userDto;
    }

    public async Task<UserContract> GetUserByEmailInternalAsync(string email)
    {
        User? user = await GetByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        UserContract? userContract = UserMap.MapToContract(user);

        return userContract;
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