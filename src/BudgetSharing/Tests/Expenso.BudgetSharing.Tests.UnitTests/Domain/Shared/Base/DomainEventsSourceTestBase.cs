using Expenso.BudgetSharing.Domain.Shared.Base;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base;

[TestFixture]
internal abstract class DomainEventsSourceTestBase : DomainTestBase<DomainEventsSource>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new DomainEventsSource();
    }
}