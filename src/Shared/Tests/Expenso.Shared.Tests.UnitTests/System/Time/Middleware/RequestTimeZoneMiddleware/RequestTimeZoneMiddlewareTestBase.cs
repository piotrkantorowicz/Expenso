using Expenso.Shared.System.Time;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Request.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Middleware.RequestTimeZoneMiddleware;

internal abstract class
    RequestTimeZoneMiddlewareTestBase : TestBase<Shared.System.Time.Middleware.RequestTimeZoneMiddleware>
{
    protected HttpContext _httpContext;
    protected Mock<RequestDelegate> _nextMock;
    protected RequestTimeZoneOptions _options;
    private Mock<ITimeZoneClock> _timeZoneClockMock;

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
        _httpContext = null!;
        _nextMock = null!;
    }

    protected void AssertRequestTimeZoneFeature(string timeZoneId, Type? providerType = null)
    {
        IRequestTimeZoneFeature? feature = _httpContext.Features.Get<IRequestTimeZoneFeature>();
        feature.Should().NotBeNull();
        feature?.RequestTimeZone.TimeZone.Id.Should().Be(expected: timeZoneId);

        if (providerType is not null)
        {
            feature?.Provider.Should().BeOfType(expectedType: providerType);
        }

        KeyValuePair<string, StringValues> timeZoneHeader =
            _httpContext.Response.Headers.FirstOrDefault(predicate: x => x.Key == _options.GetDefaultHeaderName());

        timeZoneHeader.Should().NotBeNull();
        timeZoneHeader.Value.Should().Contain(expected: timeZoneId);
    }
}