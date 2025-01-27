using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Serialization.Converters;

[TestFixture]
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
        messageContext?.ShouldNotBeNull();
        messageContext?.Name.ShouldBe(expected: "HV1qim9C");
    }
}