using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

[TestFixture]
internal sealed class Create : PermissionTypeTestBase
{
    [Test, TestCase(arg1: 0, arg2: "Unknown"), TestCase(arg1: 1, arg2: "Owner"), TestCase(arg1: 2, arg2: "SubOwner"),
     TestCase(arg1: 3, arg2: "Reviewer")]
    public void Should_CreateInstance(int value, string displayName)
    {
        // Arrange
        // Act
        BudgetSharing.Domain.Shared.ValueObjects.PermissionType result =
            BudgetSharing.Domain.Shared.ValueObjects.PermissionType.Create(value: value);

        // Assert
        result.Value.Should().Be(expected: value);
        result.DisplayName.Should().Be(expected: displayName);
    }
}