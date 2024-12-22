using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.DateTimeConverter;

[TestFixture]
internal abstract class DateTimeConverterTestBase : TestBase<Shared.System.Time.Serialization.DateTimeConverter>
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

        TestCandidate = new Shared.System.Time.Serialization.DateTimeConverter(requestTimeZone: _requestTimeZone,
            format: DateTimeFormats.Iso8601);
    }
   
    private Func<RequestTimeZone>? _requestTimeZone;
}