using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByEmailQueryQueryHandler;

[TestFixture]
internal abstract class GetUserByEmailQueryQueryHandlerTestBase : TestBase<GetUserByEmailQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userEmail = "email@email.com";

        UserRepresentation user = new()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = "email@email.com"
        };

        _getUserByEmailResponse = GetUserByEmailResponseMap.MapTo(user: user);
        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();
        TestCandidate = new GetUserByEmailQueryHandler(userService: _userServiceMock.Object);
    }

    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected string _userEmail = null!;
    protected Mock<IUserService> _userServiceMock = null!;
}