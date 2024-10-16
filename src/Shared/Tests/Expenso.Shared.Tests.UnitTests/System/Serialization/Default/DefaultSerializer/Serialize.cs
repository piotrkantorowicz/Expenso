namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Default.DefaultSerializer;

[TestFixture]
internal sealed class Serialize : DefaultSerializerTestBase
{
    [Test, TestCaseSource(sourceName: nameof(SerializedTestObjects))]
    public void Should_SerializeObject_Always(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(value: serializedObject);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Test, TestCaseSource(sourceName: nameof(SerializedTestObjects))]
    public void Should_SerializeObject_WithOptions(object serializedObject)
    {
        // Arrange
        // Act
        string result = TestCandidate.Serialize(value: serializedObject, settings: _serializerOptions);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
}