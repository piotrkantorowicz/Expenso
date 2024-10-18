namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

[TestFixture]
internal sealed class Nullable : TestBase<Shared.Domain.Types.ValueObjects.DateAndTime>
{
    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Act
        Shared.Domain.Types.ValueObjects.DateAndTime? result =
            Shared.Domain.Types.ValueObjects.DateAndTime.Nullable(value: null);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsMinValue()
    {
        // Act
        Shared.Domain.Types.ValueObjects.DateAndTime? result =
            Shared.Domain.Types.ValueObjects.DateAndTime.Nullable(value: DateTimeOffset.MinValue);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsMaxValue()
    {
        // Act
        Shared.Domain.Types.ValueObjects.DateAndTime? result =
            Shared.Domain.Types.ValueObjects.DateAndTime.Nullable(value: DateTimeOffset.MaxValue);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnsValidDateAndTime_When_ValueIsValid()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

        // Act
        Shared.Domain.Types.ValueObjects.DateAndTime? result =
            Shared.Domain.Types.ValueObjects.DateAndTime.Nullable(value: dateTimeOffset);

        // Assert
        result.Should().NotBeNull();
        result?.Value.Should().Be(expected: dateTimeOffset);
    }
}