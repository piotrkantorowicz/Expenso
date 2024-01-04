using Expenso.IAM.Core.Proxy;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy;

internal abstract class IamProxyTestBase : TestBase<IIamProxy>
{
    protected Mock<IUserService> _userServiceMock = null!;
    protected UserContract _userContract = null!;
    protected string _userId = null!;
    protected string _userEmail = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";
        _userContract = new UserContract(_userId, "Valentina", "Long", "vLong", _userEmail);
        _userServiceMock = new Mock<IUserService>();
        TestCandidate = new IamProxy(_userServiceMock.Object);
    }
}