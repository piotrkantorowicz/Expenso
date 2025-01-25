using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

using Shouldly;

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
        result.Value.ShouldBe(expected: dateTimeOffset);
    }

    [Test]
    public void Should_ThrowBusinessRuleException_When_DateTimeOffsetIsEmpty()
    {
        // Arrange
        DateTimeOffset emptyDateTimeOffset = DateTimeOffset.MinValue;

        // Act
        Action action = () => Shared.Domain.Types.ValueObjects.DateAndTime.New(value: emptyDateTimeOffset);

        // Assert
        DomainRuleValidationException? exception = action.ShouldThrow<DomainRuleValidationException>();
        exception.Message.ShouldBe(expected: "Business rule validation failed.");

        exception.Details.ShouldBe(
            expected:
            $"Empty date and time {nameof(Shared.Domain.Types.ValueObjects.DateAndTime)} cannot be processed.");
    }
}