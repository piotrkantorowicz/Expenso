using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

[TestFixture]
internal sealed class New : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId result =
            BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId.New(value: value);

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action action = () => BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId.New(value: value);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Empty identifier {nameof(BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId)} cannot be processed.");
    }
}