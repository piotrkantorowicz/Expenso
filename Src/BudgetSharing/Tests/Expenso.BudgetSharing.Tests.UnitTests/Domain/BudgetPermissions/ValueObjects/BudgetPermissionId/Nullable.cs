using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

internal sealed class Nullable : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        TestCandidate? result = TestCandidate.Nullable(value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        Guid? value = null;

        // Act
        TestCandidate? result = TestCandidate.Nullable(value);

        // Assert
        result.Should().BeNull();
    }
}