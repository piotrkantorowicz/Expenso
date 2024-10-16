using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.BudgetId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.BudgetId;

[TestFixture]
internal abstract class BudgetIdTestBase : DomainTestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
    }
}