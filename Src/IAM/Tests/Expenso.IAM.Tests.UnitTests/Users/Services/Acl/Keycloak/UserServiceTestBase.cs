using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Mappings;
using Expenso.IAM.Core.Users.Services;
using Expenso.IAM.Core.Users.Services.Acl.Keycloak;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

internal abstract class UserServiceTestBase : TestBase<IUserService>
{
    protected GetUserResponse _getUserResponse = null!;
    protected Mock<IKeycloakUserClient> _keycloakUserClientMock = null!;
    protected User _user = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;

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

        _getUserResponse = UserMap.MapToDto(_user);
        TestCandidate = new UserService(_keycloakUserClientMock.Object, new KeycloakProtectionClientOptions());
    }
}