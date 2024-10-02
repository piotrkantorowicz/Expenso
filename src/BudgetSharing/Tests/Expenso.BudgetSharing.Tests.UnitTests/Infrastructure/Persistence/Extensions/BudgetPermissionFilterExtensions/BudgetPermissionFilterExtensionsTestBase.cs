﻿using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Tests.UnitTests.Domain;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.BudgetPermissionFilterExtensions;

internal abstract class BudgetPermissionFilterExtensionsTestBase : DomainTestBase<BudgetPermission>
{
    protected BudgetPermissionId _budgetPermissionId = null!;
    protected BudgetId _budgetId = null!;
    protected PersonId _ownerId = null!;
    protected PersonId _participantId = null!;
    protected PermissionType _permissionType = null!;
    protected BudgetPermission _budgetPermission = null!;

    [SetUp]
    public void SetUp()
    {
        _budgetId = BudgetId.New(value: Guid.NewGuid());
        _ownerId = PersonId.New(value: Guid.NewGuid());
        _budgetPermissionId = BudgetPermissionId.New(value: Guid.NewGuid());
        _participantId = PersonId.New(value: Guid.NewGuid());
        _permissionType = PermissionType.Reviewer;

        _budgetPermission = BudgetPermission.Create(budgetPermissionId: _budgetPermissionId, budgetId: _budgetId,
            ownerId: _ownerId);

        _budgetPermission.AddPermission(participantId: _participantId, permissionType: _permissionType);
    }
}