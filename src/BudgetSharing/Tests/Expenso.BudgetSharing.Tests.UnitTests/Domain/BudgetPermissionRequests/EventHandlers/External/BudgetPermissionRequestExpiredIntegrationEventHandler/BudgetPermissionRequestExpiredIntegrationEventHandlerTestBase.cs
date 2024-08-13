﻿using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External.BudgetPermissionRequestExpiredIntegrationEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

internal abstract class BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase : TestBase<TestCandidate>
{
    private Mock<IBudgetPermissionRequestExpireDomainService> _budgetPermissionRequestExpireDomainServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestExpireDomainServiceMock = new Mock<IBudgetPermissionRequestExpireDomainService>();

        TestCandidate =
            new TestCandidate(
                budgetPermissionRequestExpireDomainService: _budgetPermissionRequestExpireDomainServiceMock.Object);
    }
}