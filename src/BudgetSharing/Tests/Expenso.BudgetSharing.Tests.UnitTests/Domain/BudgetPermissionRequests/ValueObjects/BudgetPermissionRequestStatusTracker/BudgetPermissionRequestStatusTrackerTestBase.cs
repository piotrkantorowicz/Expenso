using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal abstract class BudgetPermissionRequestStatusTrackerTestBase : TestBase<
    BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();

        _budgetPermissionRequestId =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(
                value: Guid.NewGuid());
    }

    protected BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId
        _budgetPermissionRequestId = null!;

    protected Mock<IClock> _clockMock = null!;
}