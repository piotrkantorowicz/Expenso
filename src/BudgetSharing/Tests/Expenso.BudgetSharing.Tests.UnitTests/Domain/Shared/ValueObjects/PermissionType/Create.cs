using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PermissionType;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

internal sealed class Create : PermissionTypeTestBase
{
    [Test, TestCase(arg1: 0, arg2: "Unknown"), TestCase(arg1: 1, arg2: "Owner"), TestCase(arg1: 2, arg2: "SubOwner"),
     TestCase(arg1: 3, arg2: "Reviewer")]
    public void Should_CreateInstance(int value, string displayName)
    {
        // Arrange
        // Act
        TestCandidate result = TestCandidate.Create(value: value);

        // Assert
        result.Value.Should().Be(expected: value);
        result.DisplayName.Should().Be(expected: displayName);
    }
}