namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Converters;

internal sealed class Deserialize : InterfaceToConcreteTypeJsonConverterTestBase
{
    [Test]
    public void Should_ReturnConcreteType()
    {
        // Arrange
        const string json = "{\"Name\":\"HV1qim9C\"}";

        // Act
        ITestInterface? messageContext =
            TestCandidate.Deserialize<ITestInterface>(value: json, settings: _serializerOptions);

        // Assert
        messageContext?.Should().NotBeNull();
        messageContext?.Name.Should().Be(expected: "HV1qim9C");
    }
}