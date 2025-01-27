using NUnit.Framework;

using Shouldly;

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
        json.ShouldContain(expected: "\"Name\":\"Test\"");
    }

    [Test]
    public void Write_ShouldThrowInvalidOperationException_WhenIncorrectTypeIsPassed()
    {
        // Arrange
        // Act
        Action action = () =>
            TestCandidate.Serialize<ITestInterface>(value: new AnotherConcreteType(), settings: _serializerOptions);

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }
}