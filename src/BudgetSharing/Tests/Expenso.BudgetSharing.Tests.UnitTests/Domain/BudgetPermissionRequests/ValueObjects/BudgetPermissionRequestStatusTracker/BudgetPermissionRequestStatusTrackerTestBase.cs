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

        DateTimeOffset submissionDate = SetupTestDates();

        TestCandidate =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);
    }

    protected BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId
        _budgetPermissionRequestId = null!;

    protected Mock<IClock> _clockMock = null!;

    protected DateTimeOffset SetupTestDates()
    {
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        return submissionDate;
    }
}