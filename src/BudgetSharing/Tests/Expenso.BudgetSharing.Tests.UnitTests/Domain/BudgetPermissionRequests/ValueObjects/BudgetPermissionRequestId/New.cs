using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

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
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action act = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(value: value);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern: $"Empty identifier {nameof(BudgetPermissionRequestId)} cannot be processed.");
    }
}