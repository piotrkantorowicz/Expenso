using Expenso.Shared.Domain.Types.Exceptions;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

internal sealed class New : DateAndTimeTestBase
{
    [Test]
    public void Should_ReturnsValidDateTimeOffset()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

        // Act
        Shared.Domain.Types.ValueObjects.DateAndTime result =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: dateTimeOffset);

        // Assert
        result.Value.Should().Be(expected: dateTimeOffset);
    }

    [Test]
    public void Should_ThrowBusinessRuleException_When_DateTimeOffsetIsEmpty()
    {
        // Arrange
        DateTimeOffset emptyDateTimeOffset = DateTimeOffset.MinValue;

        // Act & Assert
        DomainRuleValidationException? ex = Assert.Throws<DomainRuleValidationException>(code: () =>
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: emptyDateTimeOffset));

        // Assert
        Assert.That(actual: ex?.Message,
            expression: Is.EqualTo(
                expected:
                $"Empty date and time {nameof(Shared.Domain.Types.ValueObjects.DateAndTime)} cannot be processed"));
    }
}