using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

[TestFixture]
internal sealed class IsNone : PermissionTypeTestBase
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsNone()
    {
        // Arrange
        BudgetSharing.Domain.Shared.ValueObjects.PermissionType testCandidate =
            BudgetSharing.Domain.Shared.ValueObjects.PermissionType.None;

        // Act
        bool result = testCandidate.IsNone();

        // Assert
        result.Should().BeTrue();
    }
}