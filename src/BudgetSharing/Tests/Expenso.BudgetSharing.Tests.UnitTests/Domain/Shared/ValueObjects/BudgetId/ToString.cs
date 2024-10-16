using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.BudgetId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetId;

[TestFixture]
internal sealed class ToString : BudgetIdTestBase
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