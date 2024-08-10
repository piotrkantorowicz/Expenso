using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.DefaultSerializer;

internal sealed class Deserialize : DefaultSerializerTestBase
{
    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObject()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: BasicObject);

        // Act
        BasicTestObject? result = TestCandidate.Deserialize<BasicTestObject>(value: serializedObj);

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void DeserializeBasicObjectWithOptions_Always_ShouldDeserializeObject()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(value: BasicObject, settings: _serializerOptions);

        // Act
        RichTestObject? result =
            TestCandidate.Deserialize<RichTestObject>(value: serializedObj, settings: _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObjectWithCustomType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: BasicObject);

        // Act
        BasicTestObject? result =
            TestCandidate.Deserialize<BasicTestObject>(value: serializedObj, settings: typeof(BasicTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void DeserializeBasicObject_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: BasicObject);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void DeserializeBasicObjectWithOptions_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(value: BasicObject, settings: _serializerOptions);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject),
            settings: _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObject()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: ComplexObject);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(value: serializedObj);

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void DeserializeComplexObjectWithOptions_Always_ShouldDeserializeObject()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(value: ComplexObject, settings: _serializerOptions);

        // Act
        RichTestObject? result =
            TestCandidate.Deserialize<RichTestObject>(value: serializedObj, settings: _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObjectWithCustomType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: ComplexObject);

        // Act
        RichTestObject? result =
            TestCandidate.Deserialize<RichTestObject>(value: serializedObj, settings: typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void DeserializeComplexObject_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: ComplexObject);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void DeserializeComplexObjectWithOptions_Always_ShouldDeserializeObjectWithUnspecifiedType()
    {
        // Arrange
        // Act
        string serializedObj = TestCandidate.Serialize(value: ComplexObject, settings: _serializerOptions);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject),
            settings: _serializerOptions);

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }
}