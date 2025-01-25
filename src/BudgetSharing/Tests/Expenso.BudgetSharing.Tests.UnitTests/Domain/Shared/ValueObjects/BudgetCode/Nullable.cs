using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetCode;

[TestFixture]
internal sealed class Nullable : BudgetCodeTestBase
{
    [Test]
    public void Should_ReturnTypedId_When_ValueIsNotNull()
    {
        // Arrange
        const string value = "BDGT/5/12/2024";

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.BudgetCode? result =
            BudgetSharing.Domain.Shared.ValueObjects.BudgetCode.Nullable(value: value);

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public void Should_ReturnNull_When_ValueIsNull()
    {
        // Arrange
        string? value = null;

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.BudgetCode? result =
            BudgetSharing.Domain.Shared.ValueObjects.BudgetCode.Nullable(value: value);

        // Assert
        result.ShouldBeNull();
    }
}