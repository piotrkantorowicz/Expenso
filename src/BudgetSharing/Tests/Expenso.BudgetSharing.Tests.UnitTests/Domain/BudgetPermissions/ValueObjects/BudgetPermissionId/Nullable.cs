using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

[TestFixture]
internal sealed class Nullable : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        Guid value = Guid.CreateVersion7();

        // Act
        BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId? result =
            BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId.Nullable(value: value);

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        Guid? value = null;

        // Act
        BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId? result =
            BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId.Nullable(value: value);

        // Assert
        result.ShouldBeNull();
    }
}