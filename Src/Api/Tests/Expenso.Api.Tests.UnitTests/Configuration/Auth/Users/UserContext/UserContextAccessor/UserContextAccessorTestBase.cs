using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using UserAccessor = Expenso.Api.Configuration.Auth.Users.UserContext;

namespace Expenso.Api.Tests.UnitTests.Configuration.Auth.Users.UserContext.UserContextAccessor;

internal abstract class UserContextAccessorTestBase : TestBase
{
    protected UserAccessor.UserContextAccessor TestCandidate { get; private set; } = null!;

    protected Mock<IHttpContextAccessor> HttpContextAccessorMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        HttpContextAccessorMock = AutoFixtureProxy.Freeze<Mock<IHttpContextAccessor>>();
        TestCandidate = AutoFixtureProxy.Build<UserAccessor.UserContextAccessor>().WithAutoProperties().Create();
    }
}