using System.Text.Json;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization.Converters;

using Moq;

using TestCandidate = Expenso.Shared.System.Serialization.Default.DefaultSerializer;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Converters;

internal abstract class InterfaceToConcreteTypeJsonConverterTestBase : TestBase<TestCandidate>
{
    private readonly Mock<ILoggerService<TestCandidate>> _loggerMock = new();

    protected readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters =
        {
            new InterfaceToConcreteTypeJsonConverter<ITestInterface, TestConcreteType>()
        }
    };

    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate(logger: _loggerMock.Object);
    }

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