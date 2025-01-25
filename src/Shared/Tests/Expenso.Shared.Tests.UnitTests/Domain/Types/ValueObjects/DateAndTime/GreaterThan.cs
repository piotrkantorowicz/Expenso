using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

[TestFixture]
internal sealed class GreaterThan : TestBase<Shared.Domain.Types.ValueObjects.DateAndTime>
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsGreaterThanGivenDateTimeOffset()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);

        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: -1);

        // Act
        bool result = dateTimeOffset.GreaterThan(dateTimeOffset: other);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsLessThanOrEqualToGivenDateTimeOffset()
    {
        // Arrange
        Shared.Domain.Types.ValueObjects.DateAndTime dateTimeOffset =
            Shared.Domain.Types.ValueObjects.DateAndTime.New(value: DateTimeOffset.Now);

        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: 1);

        // Act
        bool result = dateTimeOffset.GreaterThan(dateTimeOffset: other);

        // Assert
        result.ShouldBeFalse();
    }
}