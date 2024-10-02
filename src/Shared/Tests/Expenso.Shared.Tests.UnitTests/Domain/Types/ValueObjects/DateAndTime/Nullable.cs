using TestCandidate = Expenso.Shared.Domain.Types.ValueObjects.DateAndTime;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

internal sealed class Nullable : TestBase<TestCandidate>
{
    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Act
        TestCandidate? result = TestCandidate.Nullable(value: null);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsMinValue()
    {
        // Act
        TestCandidate? result = TestCandidate.Nullable(value: DateTimeOffset.MinValue);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsMaxValue()
    {
        // Act
        TestCandidate? result = TestCandidate.Nullable(value: DateTimeOffset.MaxValue);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ReturnsValidDateAndTime_When_ValueIsValid()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

        // Act
        TestCandidate? result = TestCandidate.Nullable(value: dateTimeOffset);

        // Assert
        result.Should().NotBeNull();
        result?.Value.Should().Be(expected: dateTimeOffset);
    }
}