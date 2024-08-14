using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

using Microsoft.Extensions.Logging;

using Moq;

using TestCandidate = Expenso.Shared.System.Serialization.Default.DefaultSerializer;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal abstract class DefaultSerializerTestBase : TestBase<TestCandidate>
{
    protected static readonly BasicTestObject BasicObject = new()
    {
        PrimaryId = new Guid(g: "dd37661a-dc23-45ca-9a33-e92568536a73"),
        SecondaryId = 211,
        Name = "Purusnulla",
        Number = 947.38m,
        CreatedAt = DateTimeOffset.Parse(input: "2009-03-09 09:08:17")
    };

    protected static readonly RichTestObject ComplexObject = null!;

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

    private readonly Mock<ILogger<TestCandidate>> _loggerMock = new();

    protected readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }
}