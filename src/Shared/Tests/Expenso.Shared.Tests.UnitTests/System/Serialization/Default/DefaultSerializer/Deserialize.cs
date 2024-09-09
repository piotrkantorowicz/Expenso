using Expenso.Shared.Tests.UnitTests.System.Serialization.TestData;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Default.DefaultSerializer;

internal sealed class Deserialize : DefaultSerializerTestBase
{
    [Test]
    public void Should_Always_DeserializeBasicObject()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: BasicObject);

        // Act
        BasicTestObject? result = TestCandidate.Deserialize<BasicTestObject>(value: serializedObj);

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void Should_DeserializeBasicObject_WithOptions()
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
    public void Should_DeserializeBasicObject_WithCustomType()
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
    public void Should_DeserializeBasicObject_WithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: BasicObject);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: BasicObject);
    }

    [Test]
    public void Should_DeserializeBasicObject_WithUnspecifiedTypeAndOptions()
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
    public void Should_DeserializeComplexObject_Always()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: ComplexObject);

        // Act
        RichTestObject? result = TestCandidate.Deserialize<RichTestObject>(value: serializedObj);

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void Should_DeserializeComplexObject_WithOptions()
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
    public void Should_DeserializeComplexObject_WithCustomType()
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
    public void Should_DeserializeComplexObject_WithUnspecifiedType()
    {
        // Arrange
        string serializedObj = TestCandidate.Serialize(value: ComplexObject);

        // Act
        object? result = TestCandidate.Deserialize(value: serializedObj, type: typeof(RichTestObject));

        // Assert
        result.Should().BeEquivalentTo(expectation: ComplexObject);
    }

    [Test]
    public void Should_DeserializeComplexObject_WithUnspecifiedTypeAndCustomType()
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