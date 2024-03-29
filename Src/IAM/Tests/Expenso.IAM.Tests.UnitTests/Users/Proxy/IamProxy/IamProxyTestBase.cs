using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries.Dispatchers;

using TestCandidate = Expenso.IAM.Core.Application.Proxy.IamProxy;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

internal abstract class IamProxyTestBase : TestBase<IIamProxy>
{
    protected GetUserResponse _getUserResponse = null!;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _getUserResponse = new GetUserResponse(_userId, "Valentina", "Long", "vLong", _userEmail);
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        TestCandidate = new TestCandidate(_queryDispatcherMock.Object, MessageContextFactoryMock.Object);
    }
}