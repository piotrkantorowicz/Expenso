using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal sealed class Deserialize : DefaultSerializerTestBase
{
    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObject()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(BasicObject);

        // Act
        BasicTestObject? result = TestCandidate.Deserialize<BasicTestObject>(serializedObj);

        // Assert
        result.Should().BeEquivalentTo(BasicObject);
    }

    [Test]
    public void DeserializeBasicObjectWithOptions_Always_ShouldDeserializeObject()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(BasicObject, _serializerOptions);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(serializedObj, _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(BasicObject);
    }

    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObjectWithCustomType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(BasicObject);

        // Act
        BasicTestObject? result = TestCandidate.Deserialize<BasicTestObject>(serializedObj, typeof(BasicTestObject));

        // Assert
        result.Should().BeEquivalentTo(BasicObject);
    }

    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(BasicObject);

        // Act
        object? result = TestCandidate.Deserialize(serializedObj, typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(BasicObject);
    }

    [Test]
    public void DeserializeBasicObjectWithOptions_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(BasicObject, _serializerOptions);

        // Act
        object? result = TestCandidate.Deserialize(serializedObj, typeof(RichTestObject), _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(BasicObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObject()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(ComplexObject);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(serializedObj);

        // Assert
        result.Should().BeEquivalentTo(ComplexObject);
    }

    [Test]
    public void DeserializeComplexObjectWithOptions_Always_ShouldDeserializeObject()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(ComplexObject, _serializerOptions);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(serializedObj, _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(ComplexObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObjectWithCustomType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(ComplexObject);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(serializedObj, typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(ComplexObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(ComplexObject);

        // Act
        object? result = TestCandidate.Deserialize(serializedObj, typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(ComplexObject);
    }

    [Test]
    public void DeserializeComplexObjectWithOptions_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(ComplexObject, _serializerOptions);

        // Act
        object? result = TestCandidate.Deserialize(serializedObj, typeof(RichTestObject), _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(ComplexObject);
    }
}