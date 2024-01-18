using Expenso.IAM.Core.Users.Queries.GetUserInternal;
using Expenso.IAM.Core.Users.Services;
using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUserInternal;

internal abstract class GetUserInternalQueryHandlerTestBase : TestBase<GetUserInternalQueryHandler>
{
    protected Mock<IUserService> _userServiceMock = null!;
    protected GetUserInternalResponse _getUserInternalResponse = null!;
    protected string _userId = null!;
    protected string _userEmail = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _getUserInternalResponse = new GetUserInternalResponse(_userId, "Valentina", "Long", "vLong", _userEmail);
        _userServiceMock = new Mock<IUserService>();
        TestCandidate = new GetUserInternalQueryHandler(_userServiceMock.Object);
    }
}