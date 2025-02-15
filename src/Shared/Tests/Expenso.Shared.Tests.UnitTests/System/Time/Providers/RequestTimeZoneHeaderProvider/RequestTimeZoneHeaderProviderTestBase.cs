using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Inputs;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneHeaderProvider;

[TestFixture]
internal abstract class RequestTimeZoneHeaderProviderTestBase : TestBase<RequestHeaderProvider>
{
    [SetUp]
    public void SetUp()
    {
        _httpContextMock = new Mock<HttpContext>();
        _httpRequestMock = new Mock<HttpRequest>();
        _providerInputMock = new Mock<IProviderInput>();
        _httpContextMock.SetupGet(expression: c => c.Request).Returns(value: _httpRequestMock.Object);
        TestCandidate = new RequestHeaderProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _providerInputMock.Reset();
        _httpContextMock.Reset();
        _httpRequestMock.Reset();
        _providerInputMock = null!;
        _httpContextMock = null!;
        _httpRequestMock = null!;
    }

    protected Mock<HttpContext> _httpContextMock = null!;
    protected Mock<HttpRequest> _httpRequestMock = null!;
    protected Mock<IProviderInput> _providerInputMock = null!;
}