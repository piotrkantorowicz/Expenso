﻿using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

internal abstract class BudgetPermissionRequestStatusTrackerTestBase : TestBase<TestCandidate>
{
    protected BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId
        _budgetPermissionRequestId = null!;

    protected Mock<IClock> _clockMock = null!;

    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();

        _budgetPermissionRequestId =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestId.New(
                value: Guid.NewGuid());
    }
}