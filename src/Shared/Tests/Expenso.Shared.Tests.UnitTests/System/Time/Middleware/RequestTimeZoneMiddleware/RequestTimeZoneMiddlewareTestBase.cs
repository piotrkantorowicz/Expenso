using Expenso.Shared.System.Time;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Request.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Middleware.RequestTimeZoneMiddleware;

[TestFixture]
internal abstract class
    RequestTimeZoneMiddlewareTestBase : TestBase<Shared.System.Time.Middleware.RequestTimeZoneMiddleware>
{
    protected HttpContext _httpContext = null!;
    protected Mock<RequestDelegate> _nextMock = null!;
    private RequestTimeZoneOptions _options = null!;
    private Mock<ITimeZoneClock> _timeZoneClockMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContext = new DefaultHttpContext();
        _nextMock = new Mock<RequestDelegate>();
        _options = new RequestTimeZoneOptions();
        _timeZoneClockMock = new Mock<ITimeZoneClock>();

        TestCandidate = new Shared.System.Time.Middleware.RequestTimeZoneMiddleware(
            loggerFactory: NullLoggerFactory.Instance, options: _options, timeZoneClock: _timeZoneClockMock.Object);
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
        AssertResponseHeader(expectedTimeZoneId: expectedTimeZoneId);
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

    private void AssertResponseHeader(string expectedTimeZoneId)
    {
        string? headerName = _options.GetDefaultHeaderName();

        KeyValuePair<string, StringValues>? timeZoneHeader =
            _httpContext.Response.Headers.FirstOrDefault(predicate: x => x.Key == headerName);

        timeZoneHeader.ShouldNotBeNull();
        timeZoneHeader.Value.Key.ShouldBe(expected: headerName);
        timeZoneHeader.Value.Value.ShouldContain(expected: expectedTimeZoneId);
    }
}