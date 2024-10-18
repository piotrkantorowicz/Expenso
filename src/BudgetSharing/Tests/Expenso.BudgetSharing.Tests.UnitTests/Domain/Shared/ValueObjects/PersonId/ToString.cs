using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

[TestFixture]
internal sealed class ToString : PersonIdTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        BudgetSharing.Domain.Shared.ValueObjects.PersonId sut =
            BudgetSharing.Domain.Shared.ValueObjects.PersonId.New(value: value);

        // Act
        string result = sut.ToString();

        // Assert
        result.Should().Be(expected: value.ToString());
    }
}