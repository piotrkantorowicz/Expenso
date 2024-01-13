using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Core.Services.KeycloakAcl;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services;

internal abstract class KeycloakAclUserServiceTestBase : TestBase<IUserService>
{
    protected Mock<IKeycloakUserClient> _keycloakUserClientMock = null!;
    protected User _user = null!;
    protected UserDto _userDto = null!;
    protected string _userId = null!;
    protected string _userEmail = null!;

    [SetUp]
    public void SetUp()
    {
        _keycloakUserClientMock = new Mock<IKeycloakUserClient>();
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";

        _user = new User
        {
            Id = _userId,
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = _userEmail
        };

        _userDto = UserMap.MapToDto(_user);

        TestCandidate =
            new KeycloakAclUserService(_keycloakUserClientMock.Object, new KeycloakProtectionClientOptions());
    }
}