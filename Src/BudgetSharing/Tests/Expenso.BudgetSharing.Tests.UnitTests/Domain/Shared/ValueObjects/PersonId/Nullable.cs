using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PersonId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

internal sealed class Nullable : PersonIdTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        TestCandidate? result = TestCandidate.Nullable(value: value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        Guid? value = null;

        // Act
        TestCandidate? result = TestCandidate.Nullable(value: value);

        // Assert
        result.Should().BeNull();
    }
}