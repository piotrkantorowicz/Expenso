using Expenso.Shared.Domain.Types.Exceptions;

using TestCandidate = Expenso.Shared.Domain.Types.ValueObjects.DateAndTime;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

internal sealed class New : TestBase<TestCandidate>
{
    [Test]
    public void Should_ReturnsValidDateTimeOffset()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

        // Act
        TestCandidate result = TestCandidate.New(value: dateTimeOffset);

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
            TestCandidate.New(value: emptyDateTimeOffset));

        // Assert
        Assert.That(actual: ex?.Message,
            expression: Is.EqualTo(
                expected:
                $"Empty date and time {nameof(Shared.Domain.Types.ValueObjects.DateAndTime)} cannot be processed"));
    }
}