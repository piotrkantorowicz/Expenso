using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

using Microsoft.Extensions.Logging;

using Moq;

using TestCandidate = Expenso.Shared.System.Serialization.Default.DefaultSerializer;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal abstract class DefaultSerializerTestBase : TestBase<TestCandidate>
{
    private readonly Mock<ILogger<TestCandidate>> _loggerMock = new();

    protected JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    protected static readonly BasicTestObject BasicObject = new()
    {
        PrimaryId = new Guid("dd37661a-dc23-45ca-9a33-e92568536a73"),
        SecondaryId = 211,
        Name = "Purusnulla",
        Number = 947.38m,
        CreatedAt = DateTimeOffset.Parse("2009-03-09 09:08:17")
    };

    protected static readonly RichTestObject ComplexObject = new(new Guid("c6dcead1-a50d-48ea-b249-95d2e93700ac"), 119,
        "Portamaecenas", 17.38m, DateTimeOffset.Parse("2013-03-09 09:08:17"), new List<BasicTestObject>
        {
            BasicObject
        });

    protected static readonly object[] SerializedTestObjects =
    [
        new object[]
        {
            BasicObject
        },
        new object[]
        {
            ComplexObject
        }
    ];

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(_loggerMock.Object);
    }
}