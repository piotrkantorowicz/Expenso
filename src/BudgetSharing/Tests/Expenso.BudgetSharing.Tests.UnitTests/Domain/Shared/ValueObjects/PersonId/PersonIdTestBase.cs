using TestCandidate = Expenso.BudgetSharing.Domain.Shared.ValueObjects.PersonId;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.ValueObjects.PersonId;

[TestFixture]
internal abstract class PersonIdTestBase : DomainTestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
    }
}