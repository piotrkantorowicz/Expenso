using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PersonId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

internal sealed class ToString : PersonIdTestBase
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