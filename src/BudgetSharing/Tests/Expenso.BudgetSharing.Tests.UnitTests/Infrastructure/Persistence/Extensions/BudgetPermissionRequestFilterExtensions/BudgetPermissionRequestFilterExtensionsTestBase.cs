using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain;
using Expenso.Shared.System.Time;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.
    BudgetPermissionRequestFilterExtensions;

[TestFixture]
internal abstract class BudgetPermissionRequestFilterExtensionsTestBase : DomainTestBase<BudgetPermission>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _budgetId = BudgetId.New(value: Guid.CreateVersion7());
        _budgetCode = BudgetCode.New(value: "BDGT/432/12/2024");
        _status = BudgetPermissionRequestStatus.Pending;
        _budgetPermissionRequestId = BudgetPermissionRequestId.New(value: Guid.CreateVersion7());
        _participantId = PersonId.New(value: Guid.CreateVersion7());
        _ownerId = PersonId.New(value: Guid.CreateVersion7());
        _permissionType = PermissionType.Reviewer;

        _budgetPermissionRequest = BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId,
            budgetId: _budgetId, ownerId: _ownerId, personId: _participantId, permissionType: _permissionType,
            budgetCode: _budgetCode, submissionDate: _clockMock.Object.UtcNow,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: 5));
    }

    [TearDown]
    public void TearDown()
    {
        _clockMock.Reset();
        _clockMock = null!;
        _budgetId = null!;
        _budgetCode = null!;
        _budgetPermissionRequest = null!;
        _budgetPermissionRequestId = null!;
        _ownerId = null!;
        _participantId = null!;
        _permissionType = null!;
        _status = null!;
    }

    private Mock<IClock> _clockMock = null!;
    protected BudgetId _budgetId = null!;
    protected BudgetCode _budgetCode = null!;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected BudgetPermissionRequestId _budgetPermissionRequestId = null!;
    protected PersonId _ownerId = null!;
    protected PersonId _participantId = null!;
    protected PermissionType _permissionType = null!;
    protected BudgetPermissionRequestStatus _status = null!;
}