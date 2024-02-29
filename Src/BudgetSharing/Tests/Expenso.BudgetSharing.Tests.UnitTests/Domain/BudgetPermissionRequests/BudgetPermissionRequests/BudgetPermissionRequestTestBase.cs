using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal abstract class BudgetPermissionRequestTestBase : DomainTestBase<BudgetPermissionRequest>
{
    protected readonly Mock<IClock> _clockMock = new();
    protected readonly BudgetId _defaultBudgetId = BudgetId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected readonly PersonId _defaultPersonId = PersonId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected readonly PermissionType _defaultPermissionType = PermissionType.Reviewer;
    protected const int Expiration = 3;

    [SetUp]
    public void SetUp()
    {
    }

    protected BudgetPermissionRequest CreateTestCandidate(bool emitDomainEvents = false)
    {
        BudgetPermissionRequest testCandidate = BudgetPermissionRequest.Create(_defaultBudgetId, _defaultPersonId,
            _defaultPermissionType, Expiration, _clockMock.Object);

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        return testCandidate;
    }
}