namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Converters;

[TestFixture]
internal sealed class Serialize : InterfaceToConcreteTypeJsonConverterTestBase
{
    [Test]
    public void Should_SerializeInterface()
    {
        // Arrange
        ITestInterface testConcreteType = new TestConcreteType();

        // Act
        string json = TestCandidate.Serialize(value: testConcreteType, settings: _serializerOptions);

        // Assert
        json.Should().Contain(expected: "\"Name\":\"Test\"");
    }

    [Test]
    public void Write_ShouldThrowInvalidOperationException_WhenIncorrectTypeIsPassed()
    {
        // Arrange
        // Act
        Action act = () =>
            TestCandidate.Serialize<ITestInterface>(value: new AnotherConcreteType(), settings: _serializerOptions);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}