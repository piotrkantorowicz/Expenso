using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

internal sealed class New : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        TestCandidate result = TestCandidate.New(value: value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action act = () => TestCandidate.New(value: value);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed")
            .WithDetails(
                expectedWildcardPattern:
                $"Empty identifier {nameof(BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId)} cannot be processed");
    }
}