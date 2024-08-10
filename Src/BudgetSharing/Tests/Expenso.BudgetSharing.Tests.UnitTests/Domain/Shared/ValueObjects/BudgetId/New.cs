using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.BudgetId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetId;

internal sealed class New : BudgetIdTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        TestCandidate result = TestCandidate.New(value: value);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action act = () => TestCandidate.New(value: value);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Empty identifier {nameof(BudgetSharing.Domain.Shared.ValueObjects.BudgetId)} cannot be processed");
    }
}