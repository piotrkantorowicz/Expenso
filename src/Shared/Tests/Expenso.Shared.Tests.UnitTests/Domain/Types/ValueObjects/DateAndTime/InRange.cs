﻿namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

[TestFixture]
internal sealed class InRange : TestBase<Shared.Domain.Types.ValueObjects.DateAndTime>
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsWithinRange()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);
        DateTimeOffset start = dateTimeOffset.Value.AddHours(hours: -1);
        DateTimeOffset end = dateTimeOffset.Value.AddHours(hours: 1);

        // Act
        bool result = dateTimeOffset.InRange(start: start, end: end);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsNotWithinRange()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);
        DateTimeOffset start = dateTimeOffset.Value.AddHours(hours: 1);
        DateTimeOffset end = dateTimeOffset.Value.AddHours(hours: 2);

        // Act
        bool result = dateTimeOffset.InRange(start: start, end: end);

        // Assert
        result.Should().BeFalse();
    }
}