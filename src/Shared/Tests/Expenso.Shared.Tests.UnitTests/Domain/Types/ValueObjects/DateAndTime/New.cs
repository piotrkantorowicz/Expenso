﻿using Expenso.Shared.Domain.Types.Exceptions;

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

        // Act
        Action action = () => TestCandidate.New(value: emptyDateTimeOffset);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Empty date and time {nameof(Shared.Domain.Types.ValueObjects.DateAndTime)} cannot be processed");
    }
}