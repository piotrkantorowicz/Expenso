using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.Shared.UserContext;

using Microsoft.AspNetCore.Http;

using TestCandidate = Expenso.Api.Configuration.Auth.Users.UserContext;

namespace Expenso.Api.Tests.UnitTests.Configuration.Auth.Users.UserContext.UserContextAccessor;

internal abstract class UserContextAccessorTestBase : TestBase<IUserContextAccessor>
{
    protected Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        TestCandidate = new TestCandidate.UserContextAccessor(_httpContextAccessorMock.Object);
    }
}
