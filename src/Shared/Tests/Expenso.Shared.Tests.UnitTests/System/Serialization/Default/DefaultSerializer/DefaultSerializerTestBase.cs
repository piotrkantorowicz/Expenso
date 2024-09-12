using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization.Converters;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

using Moq;

using TestCandidate = Expenso.Shared.System.Serialization.Default.DefaultSerializer;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Default.DefaultSerializer;

internal abstract class DefaultSerializerTestBase : TestBase<TestCandidate>
{
    protected static readonly BasicTestObject BasicObject = new()
    {
        PrimaryId = new Guid(g: "dd37661a-dc23-45ca-9a33-e92568536a73"),
        SecondaryId = 211,
        Name = "Purusnulla",
        Number = 947.38m,
        CreatedAt = DateTimeOffset.Parse(input: "2009-03-09 09:08:17", formatProvider: CultureInfo.InvariantCulture)
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

    private readonly Mock<ILoggerService<TestCandidate>> _loggerMock = new();

    protected readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new InterfaceToConcreteTypeJsonConverter<IMessageContext, MessageContext>()
        }
    };

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }
}