using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.BudgetPermissionFilterExtensions;

[TestFixture]
internal abstract class BudgetPermissionFilterExtensionsTestBase : DomainTestBase<BudgetPermission>
{
    [SetUp]
    public void SetUp()
    {
        _budgetId = BudgetId.New(value: Guid.NewGuid());
        _budgetCode = BudgetCode.New(value: "BDGT/34/12/2024");
        _ownerId = PersonId.New(value: Guid.NewGuid());
        _budgetPermissionId = BudgetPermissionId.New(value: Guid.NewGuid());
        _participantId = PersonId.New(value: Guid.NewGuid());
        _permissionType = PermissionType.Reviewer;
        Mock<IBudgetPermissionRepository> budgetPermissionRepositoryMock = new();

        budgetPermissionRepositoryMock
            .Setup(expression: x => x.IsUnique(_budgetPermissionId, _budgetId, _ownerId,
                _budgetCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        _budgetPermission = BudgetPermission.Create(budgetPermissionId: _budgetPermissionId, budgetId: _budgetId,
            ownerId: _ownerId, budgetCode: _budgetCode,
            budgetPermissionRepository: budgetPermissionRepositoryMock.Object);

        _budgetPermission.AddPermission(participantId: _participantId, permissionType: _permissionType);
    }

    [TearDown]
    public void TearDown()
    {
        _budgetId = null!;
        _budgetCode = null!;
        _budgetPermission = null!;
        _budgetPermissionId = null!;
        _ownerId = null!;
        _participantId = null!;
        _permissionType = null!;
    }

    protected BudgetId _budgetId = null!;
    protected BudgetCode _budgetCode = null!;
    protected BudgetPermission _budgetPermission = null!;
    protected BudgetPermissionId _budgetPermissionId = null!;
    protected PersonId _ownerId = null!;
    protected PersonId _participantId = null!;
    protected PermissionType _permissionType = null!;
}