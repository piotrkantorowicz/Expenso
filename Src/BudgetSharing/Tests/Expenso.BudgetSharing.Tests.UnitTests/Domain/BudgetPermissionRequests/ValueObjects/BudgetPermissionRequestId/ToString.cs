using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId;

internal sealed class ToString : BudgetPermissionRequestIdTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        Guid value = Guid.NewGuid();
        TestCandidate sut = TestCandidate.New(value);

        // Act
        string result = sut.ToString();

        // Assert
        result.Should().Be(value.ToString());
    }
}