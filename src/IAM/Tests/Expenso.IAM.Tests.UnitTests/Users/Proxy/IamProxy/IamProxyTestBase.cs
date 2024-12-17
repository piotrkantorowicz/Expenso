using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Sdk.Admin.Models;

using Moq;

using NUnit.Framework;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal abstract class IamProxyTestBase : TestBase<IIamProxy>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";

        UserRepresentation user = new()
        {
            Id = _userId,
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = _userEmail
        };

        _getUserByIdResponse = GetUserByIdResponseMap.MapTo(user: user);
        _getUserByEmailResponse = GetUserByEmailResponseMap.MapTo(user: user);
        _getUsersResponse = [GetUsersResponseMap.MapTo(user: user)];
        _queryDispatcherMock = new Mock<IQueryDispatcher>();

        TestCandidate = new Core.Application.Proxy.IamProxy(queryDispatcher: _queryDispatcherMock.Object,
            messageContextFactory: MessageContextFactoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _queryDispatcherMock.Reset();
        _getUserByIdResponse = null!;
        _getUserByEmailResponse = null!;
        _getUsersResponse = null!;
        _queryDispatcherMock = null!;
        _userId = null!;
        _userEmail = null!;
        TestCandidate = null!;
    }

    protected GetUserByIdResponse _getUserByIdResponse = null!;
    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected IReadOnlyCollection<GetUsersResponse> _getUsersResponse = null!;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
}