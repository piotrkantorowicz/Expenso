using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Inputs;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneCookieProvider;

[TestFixture]
internal abstract class RequestTimeZoneCookieProviderTestBase : TestBase<RequestCookieProvider>
{
    [SetUp]
    public void SetUp()
    {
        _httpContextMock = new Mock<HttpContext>();
        _httpRequestMock = new Mock<HttpRequest>();
        _providerInputMock = new Mock<IProviderInput>();
        _cookiesMock = new Mock<IRequestCookieCollection>();
        _httpContextMock.SetupGet(expression: c => c.Request).Returns(value: _httpRequestMock.Object);
        _httpRequestMock.SetupGet(expression: r => r.Cookies).Returns(value: _cookiesMock.Object);
        TestCandidate = new RequestCookieProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _httpContextMock.Reset();
        _httpRequestMock.Reset();
        _cookiesMock.Reset();
        _providerInputMock.Reset();
        _providerInputMock = null!;
        _httpContextMock = null!;
        _httpRequestMock = null!;
        _cookiesMock = null!;
    }

    protected Mock<HttpContext> _httpContextMock = null!;
    protected Mock<IRequestCookieCollection> _cookiesMock = null!;
    protected Mock<IProviderInput> _providerInputMock = null!;
    private Mock<HttpRequest> _httpRequestMock = null!;
}