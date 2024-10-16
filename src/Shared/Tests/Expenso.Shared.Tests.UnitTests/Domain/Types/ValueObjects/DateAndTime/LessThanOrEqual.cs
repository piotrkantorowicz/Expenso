using TestCandidate = Expenso.Shared.Domain.Types.ValueObjects.DateAndTime;

namespace Expenso.Shared.Tests.UnitTests.Domain.Types.ValueObjects.DateAndTime;

[TestFixture]
internal sealed class LessThanOrEqual : TestBase<TestCandidate>
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsLessThanOrEqualToGivenDateTimeOffset()
    {
        // Arrange
        TestCandidate dateTimeOffset = TestCandidate.New(value: DateTimeOffset.Now);
        DateTimeOffset other = dateTimeOffset.Value;

        // Act
        bool result = dateTimeOffset <= other;

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsGreaterThanGivenDateTimeOffset()
    {
        // Arrange
        TestCandidate dateTimeOffset = TestCandidate.New(value: DateTimeOffset.Now);
        DateTimeOffset other = dateTimeOffset.Value.AddHours(hours: -1);

        // Act
        bool result = dateTimeOffset.LessThanOrEqual(dateTimeOffset: other);

        // Assert
        result.Should().BeFalse();
    }
}