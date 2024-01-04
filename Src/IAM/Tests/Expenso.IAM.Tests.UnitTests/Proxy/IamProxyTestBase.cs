using Expenso.IAM.Core.Proxy;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy;

internal abstract class IamProxyTestBase : TestBase
{
    protected IIamProxy TestCandidate { get; private set; } = null!;

    protected Mock<IUserService> UserServiceMock { get; private set; } = null!;

    protected string UserId { get; private set; } = null!;

    protected string UserEmail { get; private set; } = null!;

    protected UserContract UserContract { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        UserId = Guid.NewGuid().ToString();
        UserEmail = "email@email.com";
        UserContract = new UserContract(UserId, "Valentina", "Long", "vLong", UserEmail);
        UserServiceMock = new Mock<IUserService>();
        TestCandidate = new IamProxy(UserServiceMock.Object);
    }
}