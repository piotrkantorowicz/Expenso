using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId;

[TestFixture]
internal sealed class ToString : BudgetPermissionRequestIdTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId sut =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(value: value);

        // Act
        string result = sut.ToString();

        // Assert
        result.Should().Be(expected: value.ToString());
    }
}