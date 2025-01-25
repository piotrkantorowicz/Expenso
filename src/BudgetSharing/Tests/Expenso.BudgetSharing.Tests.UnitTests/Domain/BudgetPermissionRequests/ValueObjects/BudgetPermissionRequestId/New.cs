using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId;

[TestFixture]
internal sealed class New : BudgetPermissionRequestIdTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId result =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(value: value);

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(value: value);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Empty identifier {nameof(BudgetPermissionRequestId)} cannot be processed.");
    }
}