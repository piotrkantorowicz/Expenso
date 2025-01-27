using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization.Converters;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Default.DefaultSerializer;

[TestFixture]
internal abstract class DefaultSerializerTestBase : TestBase<Shared.System.Serialization.Default.DefaultSerializer>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new Shared.System.Serialization.Default.DefaultSerializer(logger: _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }

    protected static readonly BasicTestObject BasicObject = new()
    {
        PrimaryId = new Guid(g: "0194a81a-48c6-7974-9978-f235d102a11a"),
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

    private readonly Mock<ILoggerService<Shared.System.Serialization.Default.DefaultSerializer>> _loggerMock = new();

    protected readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new InterfaceToConcreteTypeJsonConverter<IMessageContext, MessageContext>()
        }
    };
}