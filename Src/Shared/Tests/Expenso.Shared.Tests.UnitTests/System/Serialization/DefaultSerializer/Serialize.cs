namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal sealed class Serialize : DefaultSerializerTestBase
{
    [Test, TestCaseSource(nameof(SerializedTestObjects))]
    public void Serialize_Always_ShouldSerializeObject(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(serializedObject);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test, TestCaseSource(nameof(SerializedTestObjects))]
    public void SerializeWithOptions_Always_ShouldSerializeObject(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(serializedObject, _serializerOptions);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
}