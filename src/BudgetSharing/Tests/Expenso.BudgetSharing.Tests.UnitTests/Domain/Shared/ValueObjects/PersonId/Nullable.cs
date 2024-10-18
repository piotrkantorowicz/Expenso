using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

[TestFixture]
internal sealed class Nullable : PersonIdTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.PersonId? result =
            BudgetSharing.Domain.Shared.ValueObjects.PersonId.Nullable(value: value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        Guid? value = null;

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.PersonId? result =
            BudgetSharing.Domain.Shared.ValueObjects.PersonId.Nullable(value: value);

        // Assert
        result.Should().BeNull();
    }
}