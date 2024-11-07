using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUsersQueryHandler;

[TestFixture]
internal abstract class
    GetUsersQueryHandlerTestBase : TestBase<Core.Application.Users.Read.Queries.GetUsers.GetUsersQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();

        IEnumerable<UserRepresentation> users =
        [
            new()
            {
                Id = _userId,
                FirstName = "Valentina",
                LastName = "Long",
                Username = "vLong",
                Email = "email@email.com"
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Brenda",
                LastName = "Mai",
                Username = "BrendaMai",
                Email = "mai@email.com"
            }
        ];

        _getUserByIdResponse = GetUsersResponseMap.MapTo(users: users);
        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();

        TestCandidate =
            new Core.Application.Users.Read.Queries.GetUsers.GetUsersQueryHandler(userService: _userServiceMock.Object);
    }

    protected IReadOnlyCollection<GetUsersResponse> _getUserByIdResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected string _userId = null!;
    protected Mock<IUserService> _userServiceMock = null!;
}