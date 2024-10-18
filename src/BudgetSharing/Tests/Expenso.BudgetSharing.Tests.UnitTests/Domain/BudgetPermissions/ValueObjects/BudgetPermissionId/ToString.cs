using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId;

[TestFixture]
internal sealed class ToString : BudgetPermissionIdTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId sut =
            BudgetSharing.Domain.BudgetPermissions.ValueObjects.BudgetPermissionId.New(value: value);

        // Act
        string result = sut.ToString();

        // Assert
        result.Should().Be(expected: value.ToString());
    }
}