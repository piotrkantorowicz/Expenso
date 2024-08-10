using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

internal sealed class ToString : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        Guid value = Guid.NewGuid();
        TestCandidate sut = TestCandidate.New(value: value);

        // Act
        string result = sut.ToString();

        // Assert
        result.Should().Be(expected: value.ToString());
    }
}