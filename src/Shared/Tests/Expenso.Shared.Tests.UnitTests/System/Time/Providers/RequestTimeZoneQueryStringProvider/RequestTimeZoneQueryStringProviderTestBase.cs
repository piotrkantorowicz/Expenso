using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneQueryStringProvider;

[TestFixture]
internal abstract class
    RequestTimeZoneQueryStringProviderTestBase : TestBase<
    Shared.System.Time.Providers.RequestTimeZoneQueryStringProvider>
{
    protected Mock<HttpContext> _httpContextMock;
    protected Mock<HttpRequest> _httpRequestMock;

    [SetUp]
    public void SetUp()
    {
        _httpContextMock = new Mock<HttpContext>();
        _httpRequestMock = new Mock<HttpRequest>();
        _httpContextMock.SetupGet(expression: c => c.Request).Returns(value: _httpRequestMock.Object);
        TestCandidate = new Shared.System.Time.Providers.RequestTimeZoneQueryStringProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _httpContextMock.Reset();
        _httpRequestMock.Reset();
        _httpContextMock = null!;
        _httpRequestMock = null!;
    }
}