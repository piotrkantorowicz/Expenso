using System.Text.Json;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization.Converters;
using Expenso.Shared.System.Serialization.Default;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Converters;

[TestFixture]
internal abstract class InterfaceToConcreteTypeJsonConverterTestBase : TestBase<DefaultSerializer>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new DefaultSerializer(logger: _loggerMock.Object);
    }

    private readonly Mock<ILoggerService<DefaultSerializer>> _loggerMock = new();

    protected readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters =
        {
            new InterfaceToConcreteTypeJsonConverter<ITestInterface, TestConcreteType>()
        }
    };

    [Test]
    public void SerializeAndDeserializeAreSymmetric()
    {
        // Arrange
        ITestInterface originalMessageContext = new TestConcreteType();

        // Act
        string json = TestCandidate.Serialize(value: originalMessageContext, settings: _serializerOptions);

        ITestInterface? deserializedMessageContext =
            TestCandidate.Deserialize<ITestInterface>(value: json, settings: _serializerOptions);

        // Assert
        // Assert
        deserializedMessageContext?.Should().NotBeNull();
        deserializedMessageContext?.Name.Should().Be(expected: originalMessageContext.Name);
    }

    protected interface ITestInterface
    {
        string Name { get; }
    }

    protected sealed class TestConcreteType : ITestInterface
    {
        public string Name { get; set; } = "Test";
    }

    protected sealed class AnotherConcreteType : ITestInterface
    {
        public string Name { get; set; } = "Another";
    }
}