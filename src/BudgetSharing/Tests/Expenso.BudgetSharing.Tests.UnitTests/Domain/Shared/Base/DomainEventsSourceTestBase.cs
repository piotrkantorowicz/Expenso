using TestCandidate = Expenso.BudgetSharing.Domain.Shared.Base.DomainEventsSource;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base;

internal abstract class DomainEventsSourceTestBase : DomainTestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new TestCandidate();
    }
}