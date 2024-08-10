namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal sealed class Serialize : DefaultSerializerTestBase
{
    [Test, TestCaseSource(sourceName: nameof(SerializedTestObjects))]
    public void Serialize_Always_ShouldSerializeObject(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(value: serializedObject);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test, TestCaseSource(sourceName: nameof(SerializedTestObjects))]
    public void SerializeWithOptions_Always_ShouldSerializeObject(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(value: serializedObject, settings: _serializerOptions);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
}