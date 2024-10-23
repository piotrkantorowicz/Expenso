﻿namespace Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Request;

public sealed record GetBudgetPermissionsRequest(
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionsRequest_PermissionType? PermissionType = null);