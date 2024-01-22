using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Queries.GetUser;
using Expenso.IAM.Core.Users.Services;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser;

internal abstract class GetUserQueryHandlerTestBase : TestBase<GetUserQueryHandler>
{
    protected GetUserResponse _getUserResponse = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
    protected Mock<IUserService> _userServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _getUserResponse = new GetUserResponse(_userId, "Valentina", "Long", "vLong", _userEmail);
        _userServiceMock = new Mock<IUserService>();
        TestCandidate = new GetUserQueryHandler(_userServiceMock.Object);
    }
}