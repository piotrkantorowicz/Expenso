using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

[TestFixture]
internal abstract class BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase : TestBase<BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External.BudgetPermissionRequestExpiredIntegrationEventHandler>
{
    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestExpireDomainServiceMock = new Mock<IBudgetPermissionRequestExpirationDomainService>();

        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External.BudgetPermissionRequestExpiredIntegrationEventHandler(
                    budgetPermissionRequestExpirationDomainService: _budgetPermissionRequestExpireDomainServiceMock
                        .Object);
    }

    [TearDown]
    public void TearDown()
    {
        _budgetPermissionRequestExpireDomainServiceMock.Reset();
        _budgetPermissionRequestExpireDomainServiceMock = null!;
        TestCandidate = null!;
    }

    private Mock<IBudgetPermissionRequestExpirationDomainService> _budgetPermissionRequestExpireDomainServiceMock =
        null!;
}