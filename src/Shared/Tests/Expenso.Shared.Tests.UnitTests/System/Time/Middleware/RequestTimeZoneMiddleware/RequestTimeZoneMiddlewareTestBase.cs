using Expenso.Shared.System.Time;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Request.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Middleware.RequestTimeZoneMiddleware;

[TestFixture]
internal abstract class
    RequestTimeZoneMiddlewareTestBase : TestBase<Shared.System.Time.Middleware.RequestTimeZoneMiddleware>
{
    protected HttpContext _httpContext = null!;
    private RequestTimeZoneOptions _options = null!;
    private Mock<ITimeZoneClock> _timeZoneClockMock = null!;
    private Mock<RequestDelegate> _nextMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContext = new DefaultHttpContext();
        _nextMock = new Mock<RequestDelegate>();
        _options = new RequestTimeZoneOptions();
        _timeZoneClockMock = new Mock<ITimeZoneClock>();

        TestCandidate = new Shared.System.Time.Middleware.RequestTimeZoneMiddleware(
            loggerFactory: NullLoggerFactory.Instance, options: _options, timeZoneClock: _timeZoneClockMock.Object,
            next: _nextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _nextMock.Reset();
        _timeZoneClockMock.Reset();
        _httpContext = null!;
        _nextMock = null!;
        _options = null!;
        _timeZoneClockMock = null!;
    }

    protected void AssertRequestTimeZoneFeature(string expectedTimeZoneId, Type? expectedProviderType = null)
    {
        IRequestTimeZoneFeature? feature = _httpContext.Features.Get<IRequestTimeZoneFeature>();
        AssertFeatureExists(feature: feature);
        AssertTimeZoneId(feature: feature!, expectedTimeZoneId: expectedTimeZoneId);
        AssertProviderType(feature: feature!, expectedProviderType: expectedProviderType);

        // Cookie, Header, QueryString will be tested as part of integration tests
    }

    private static void AssertFeatureExists(IRequestTimeZoneFeature? feature)
    {
        feature.ShouldNotBeNull();
    }

    private static void AssertTimeZoneId(IRequestTimeZoneFeature feature, string expectedTimeZoneId)
    {
        feature.RequestTimeZone.ShouldNotBeNull();
        feature.RequestTimeZone.TimeZone.ShouldNotBeNull();
        feature.RequestTimeZone.TimeZone.Id.ShouldBe(expected: expectedTimeZoneId);
    }

    private static void AssertProviderType(IRequestTimeZoneFeature feature, Type? expectedProviderType)
    {
        if (expectedProviderType is null)
        {
            return;
        }

        feature.Provider.ShouldNotBeNull();
        feature.Provider.ShouldBeOfType(expected: expectedProviderType);
    }
}