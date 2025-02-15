using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.DateTimeOffsetConverter;

[TestFixture]
internal abstract class
    DateTimeOffsetConverterTestBase : TestBase<Shared.System.Time.Serialization.DateTimeOffsetConverter>
{
    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }

    protected void CreateTestCandidate(string timeZoneName = TimeZoneIds.Utc,
        string format = DateTimeFormats.Iso8601TimeZone)
    {
        _requestTimeZone = () => new RequestTimeZone(name: timeZoneName);
        _requestDateTimeOffsetFormat = () => new RequestDateTimeOffsetFormat(format: format);

        TestCandidate = new Shared.System.Time.Serialization.DateTimeOffsetConverter(requestTimeZone: _requestTimeZone,
            requestDateTimeOffsetFormat: _requestDateTimeOffsetFormat);
    }

    private Func<RequestTimeZone>? _requestTimeZone;
    private Func<RequestDateTimeOffsetFormat>? _requestDateTimeOffsetFormat;
}