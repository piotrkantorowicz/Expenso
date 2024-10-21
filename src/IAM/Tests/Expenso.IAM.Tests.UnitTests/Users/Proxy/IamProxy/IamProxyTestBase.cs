using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.Queries.Dispatchers;

using Keycloak.AuthServices.Sdk.Admin.Models;

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
        _queryDispatcherMock = new Mock<IQueryDispatcher>();

        TestCandidate = new Core.Application.Proxy.IamProxy(queryDispatcher: _queryDispatcherMock.Object,
            messageContextFactory: MessageContextFactoryMock.Object);
    }

    protected GetUserByIdResponse _getUserByIdResponse = null!;
    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
}