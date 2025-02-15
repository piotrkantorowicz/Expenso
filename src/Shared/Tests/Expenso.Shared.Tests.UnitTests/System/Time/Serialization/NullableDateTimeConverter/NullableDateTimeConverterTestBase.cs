using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.NullableDateTimeConverter;

[TestFixture]
internal abstract class
    NullableDateTimeConverterTestBase : TestBase<Shared.System.Time.Serialization.NullableDateTimeConverter>
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

    protected void CreateTestCandidate(string timeZoneName = TimeZoneIds.Utc, string format = DateTimeFormats.Iso8601)
    {
        _requestTimeZone = () => new RequestTimeZone(name: timeZoneName);
        _requestDateTimeFormat = () => new RequestDateTimeFormat(format: format);

        TestCandidate =
            new Shared.System.Time.Serialization.NullableDateTimeConverter(requestTimeZone: _requestTimeZone,
                requestDateTimeFormat: _requestDateTimeFormat);
    }

    private Func<RequestTimeZone>? _requestTimeZone;
    private Func<RequestDateTimeFormat>? _requestDateTimeFormat;
}