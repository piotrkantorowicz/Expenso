using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PermissionType;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PermissionType;

internal sealed class IsUnknown : PermissionTypeTestBase
{
    [Test]
    public void Should_ReturnTrue_When_ValueIsUnknown()
    {
        // Arrange
        TestCandidate testCandidate = TestCandidate.Unknown;

        // Act
        bool result = testCandidate.IsUnknown();

        // Assert
        result.Should().BeTrue();
    }
}