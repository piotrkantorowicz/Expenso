namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

internal sealed class GreaterThanOrEqual : DateAndTimeTestBase
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsGreaterThanOrEqualToGivenDateTimeOffset()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);

        DateTimeOffset other = dateTimeOffset.Value;

        // Act
        bool result = dateTimeOffset.GreaterThanOrEqual(dateTimeOffset: other);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsLessThanGivenDateTimeOffset()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);

        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: 1);

        // Act
        bool result = dateTimeOffset.GreaterThanOrEqual(dateTimeOffset: other);

        // Assert
        result.Should().BeFalse();
    }
}