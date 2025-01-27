using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

[TestFixture]
internal sealed class New : PersonIdTestBase
{
    [Test]
    public void Should_CreateTypedId()
    {
        // Arrange
        Guid value = Guid.CreateVersion7();

        // Act
        BudgetSharing.Domain.Shared.ValueObjects.PersonId result =
            BudgetSharing.Domain.Shared.ValueObjects.PersonId.New(value: value);

        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ValueIsEmpty()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        Action action = () => BudgetSharing.Domain.Shared.ValueObjects.PersonId.New(value: value);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Empty identifier {nameof(BudgetSharing.Domain.Shared.ValueObjects.PersonId)} cannot be processed.");
    }
}