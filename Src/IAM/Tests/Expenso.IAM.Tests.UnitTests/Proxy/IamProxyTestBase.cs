using Expenso.IAM.Core.Users.Proxy;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Tests.UnitTests.Proxy;

internal abstract class IamProxyTestBase : TestBase<IIamUsersProxy>
{
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected GetUserInternalResponse _getUserInternalResponse = null!;
    protected string _userId = null!;
    protected string _userEmail = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _getUserInternalResponse = new GetUserInternalResponse(_userId, "Valentina", "Long", "vLong", _userEmail);
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        TestCandidate = new IamUsersUsersProxy(_queryDispatcherMock.Object);
    }
}