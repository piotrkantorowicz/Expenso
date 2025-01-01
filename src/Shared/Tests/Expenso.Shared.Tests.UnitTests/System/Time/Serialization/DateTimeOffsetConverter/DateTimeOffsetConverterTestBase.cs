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

    protected void CreateTestCandidate(string timeZoneName = TimeZoneIds.Utc)
    {
        _requestTimeZone = () => new RequestTimeZone(name: timeZoneName);

        TestCandidate = new Shared.System.Time.Serialization.DateTimeOffsetConverter(requestTimeZone: _requestTimeZone,
            supportedFormats: [DateTimeFormats.Iso8601TimeZone]);
    }

    private Func<RequestTimeZone>? _requestTimeZone;
}