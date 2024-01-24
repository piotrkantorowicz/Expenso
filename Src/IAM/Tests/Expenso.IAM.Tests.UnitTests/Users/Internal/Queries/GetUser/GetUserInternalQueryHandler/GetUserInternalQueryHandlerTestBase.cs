using Expenso.IAM.Core.Users.Services;
using Expenso.IAM.Proxy.DTO.GetUser;

using TestCandidate = Expenso.IAM.Core.Users.Internal.Queries.GetUser.GetUserInternalQueryHandler;

namespace Expenso.IAM.Tests.UnitTests.Users.Internal.Queries.GetUser.GetUserInternalQueryHandler;

internal abstract class GetUserInternalQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected GetUserInternalResponse _getUserInternalResponse = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
    protected Mock<IUserService> _userServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _getUserInternalResponse = new GetUserInternalResponse(_userId, "Valentina", "Long", "vLong", _userEmail);
        _userServiceMock = new Mock<IUserService>();
        TestCandidate = new TestCandidate(_userServiceMock.Object);
    }
}
