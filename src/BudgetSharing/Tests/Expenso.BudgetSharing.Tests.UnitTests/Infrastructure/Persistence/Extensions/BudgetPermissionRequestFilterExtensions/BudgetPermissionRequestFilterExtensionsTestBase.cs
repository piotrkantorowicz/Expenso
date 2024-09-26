using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.
    BudgetPermissionRequestFilterExtensions;

internal abstract class BudgetPermissionRequestFilterExtensionsTestBase : DomainTestBase<BudgetPermission>
{
    private Mock<IClock> _clockMock = null!;
    protected BudgetPermissionRequestId _budgetPermissionRequestId = null!;
    protected BudgetId _budgetId = null!;
    protected PersonId _participantId = null!;
    protected PersonId _ownerId = null!;
    protected PermissionType _permissionType = null!;
    protected BudgetPermissionRequestStatus _status = null!;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;

    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _budgetId = BudgetId.New(value: Guid.NewGuid());
        _status = BudgetPermissionRequestStatus.Pending;
        _budgetPermissionRequestId = BudgetPermissionRequestId.New(value: Guid.NewGuid());
        _participantId = PersonId.New(value: Guid.NewGuid());
        _ownerId = PersonId.New(value: Guid.NewGuid());
        _permissionType = PermissionType.Reviewer;

        _budgetPermissionRequest = BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId,
            budgetId: _budgetId, ownerId: _ownerId, personId: _participantId, permissionType: _permissionType,
            clock: _clockMock.Object, expirationDays: 5);

        ;
    }
}