using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByIdQueryHandler;

[TestFixture]
internal abstract class GetUserByIdQueryHandlerTestBase : TestBase<
    GetUserByIdQueryQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();

        UserRepresentation user = new()
        {
            Id = _userId,
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = "email@email.com"
        };

        _getUserByIdResponse = GetUserByIdResponseMap.MapTo(user: user);
        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();

        TestCandidate =
            new GetUserByIdQueryQueryHandler(
                userService: _userServiceMock.Object);
    }

    protected GetUserByIdResponse _getUserByIdResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected string _userId = null!;
    protected Mock<IUserService> _userServiceMock = null!;
}