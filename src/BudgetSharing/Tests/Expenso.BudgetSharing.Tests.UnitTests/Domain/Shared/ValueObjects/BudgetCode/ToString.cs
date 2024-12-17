using FluentAssertions;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetCode;

[TestFixture]
internal sealed class ToString : BudgetCodeTestBase
{
    [Test]
    public void Should_ReturnString()
    {
        // Arrange
        const string value = "BDGT/5/12/2024";

        BudgetSharing.Domain.Shared.ValueObjects.BudgetCode testCandidate =
            BudgetSharing.Domain.Shared.ValueObjects.BudgetCode.New(value: value);

        // Act
        string result = testCandidate.ToString();

        // Assert
        result.Should().Be(expected: value);
    }
}