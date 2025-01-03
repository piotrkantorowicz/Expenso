using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneCookieProvider;

[TestFixture]
internal abstract class
    RequestTimeZoneCookieProviderTestBase : TestBase<Shared.System.Time.Providers.RequestTimeZoneCookieProvider>
{
    protected Mock<HttpContext> _httpContextMock = null!;
    protected Mock<IRequestCookieCollection> _cookiesMock = null!;
    private Mock<HttpRequest> _httpRequestMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextMock = new Mock<HttpContext>();
        _httpRequestMock = new Mock<HttpRequest>();
        _cookiesMock = new Mock<IRequestCookieCollection>();
        _httpContextMock.SetupGet(expression: c => c.Request).Returns(value: _httpRequestMock.Object);
        _httpRequestMock.SetupGet(expression: r => r.Cookies).Returns(value: _cookiesMock.Object);
        TestCandidate = new Shared.System.Time.Providers.RequestTimeZoneCookieProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _httpContextMock.Reset();
        _httpRequestMock.Reset();
        _cookiesMock.Reset();
        _httpContextMock = null!;
        _httpRequestMock = null!;
        _cookiesMock = null!;
    }
}