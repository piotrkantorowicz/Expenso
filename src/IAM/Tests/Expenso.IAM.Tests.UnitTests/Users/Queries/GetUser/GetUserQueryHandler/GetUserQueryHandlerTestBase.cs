using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserQueryHandler;

[TestFixture]
internal abstract class
    GetUserQueryHandlerTestBase : TestBase<Core.Application.Users.Read.Queries.GetUser.GetUserQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";

        _getUserResponse = new GetUserResponse(UserId: _userId, Firstname: "Valentina", Lastname: "Long",
            Username: "vLong", Email: _userEmail);

        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();

        TestCandidate =
            new Core.Application.Users.Read.Queries.GetUser.GetUserQueryHandler(userService: _userServiceMock.Object);
    }

    protected GetUserResponse _getUserResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
    protected Mock<IUserService> _userServiceMock = null!;
}