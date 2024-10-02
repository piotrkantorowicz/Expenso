﻿using TestCandidate = Expenso.Shared.Domain.Types.ValueObjects.DateAndTime;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

internal sealed class LessThan : TestBase<TestCandidate>
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsLessThanGivenDateTimeOffset()
    {
        // Arrange
        TestCandidate dateTimeOffset = TestCandidate.New(value: DateTimeOffset.Now);
        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: 1);

        // Act
        bool result = dateTimeOffset.LessThan(dateTimeOffset: other);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsGreaterThanOrEqualToGivenDateTimeOffset()
    {
        // Arrange
        TestCandidate dateTimeOffset = TestCandidate.New(value: DateTimeOffset.Now);
        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: -1);

        // Act
        bool result = dateTimeOffset.LessThan(dateTimeOffset: other);

        // Assert
        result.Should().BeFalse();
    }
}