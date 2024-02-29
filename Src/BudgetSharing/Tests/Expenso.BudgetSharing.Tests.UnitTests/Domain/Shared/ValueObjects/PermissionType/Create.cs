using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PermissionType;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

internal sealed class Create : PermissionTypeTestBase
{
    [Test, TestCase(0, "Unknown"), TestCase(1, "Owner"), TestCase(2, "SubOwner"), TestCase(3, "Reviewer")]
    public void Should_CreateInstance(int value, string displayName)
    {
        // Arrange
        // Act
        TestCandidate result = TestCandidate.Create(value);

        // Assert
        result.Value.Should().Be(value);
        result.DisplayName.Should().Be(displayName);
    }
}