using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

[TestFixture]
internal sealed class IsUnknown : PermissionTypeTestBase
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsUnknown()
    {
        // Arrange
        BudgetSharing.Domain.Shared.ValueObjects.PermissionType testCandidate =
            BudgetSharing.Domain.Shared.ValueObjects.PermissionType.Unknown;

        // Act
        bool result = testCandidate.IsUnknown();

        // Assert
        result.Should().BeTrue();
    }
}