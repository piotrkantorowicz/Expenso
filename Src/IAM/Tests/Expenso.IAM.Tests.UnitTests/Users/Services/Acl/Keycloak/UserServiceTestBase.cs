﻿using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser.DTO.Response.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;
using Expenso.IAM.Proxy.DTO.GetUser;

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

        _getUserResponse = GetUserResponseMap.MapTo(_user);
        TestCandidate = new UserService(_keycloakUserClientMock.Object, new KeycloakProtectionClientOptions());
    }
}