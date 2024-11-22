using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetCode;

[TestFixture]
internal sealed class New : BudgetCodeTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        const string value = "BDGT/100/12/2024";

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.BudgetCode result =
            BudgetSharing.Domain.Shared.ValueObjects.BudgetCode.New(value: value);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(expected: value);
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "BDGT/0/12/2024"),
     TestCase(arg: "BDGT/100001/12/2024"), TestCase(arg: "BDGT/5/0/2024"), TestCase(arg: "BDGT/5/13/2024"),
     TestCase(arg: "BDGT/5/12/1999"), TestCase(arg: "BDGT/5/12/3000")]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsInvalid(string value)
    {
        // Arrange
        // Act
        Action act = () => BudgetSharing.Domain.Shared.ValueObjects.BudgetCode.New(value: value);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(expectedWildcardPattern: $"Budget code {value} must have correct format.");
    }
}