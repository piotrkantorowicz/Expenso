using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId;

[TestFixture]
internal sealed class Nullable : BudgetPermissionRequestIdTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId? result =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.Nullable(value: value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        Guid? value = null;

        // Act
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId? result =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.Nullable(value: value);

        // Assert
        result.Should().BeNull();
    }
}