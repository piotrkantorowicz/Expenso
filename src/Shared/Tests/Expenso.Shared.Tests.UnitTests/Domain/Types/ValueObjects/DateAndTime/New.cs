using Expenso.Shared.Domain.Types.Exceptions;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

[TestFixture]
internal sealed class New : TestBase<Shared.Domain.Types.ValueObjects.DateAndTime>
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

        // Act
        Action action = () => Shared.Domain.Types.ValueObjects.DateAndTime.New(value: emptyDateTimeOffset);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .Where(exceptionExpression: x => x.Details ==
                                             $"Empty date and time {nameof(Shared.Domain.Types.ValueObjects.DateAndTime)} cannot be processed.");
    }
}