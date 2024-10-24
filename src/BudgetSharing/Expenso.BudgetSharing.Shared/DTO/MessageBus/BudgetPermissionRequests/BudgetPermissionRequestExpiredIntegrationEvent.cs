﻿using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;

public sealed record BudgetPermissionRequestExpiredIntegrationEvent(
    IMessageContext MessageContext,
    Guid BudgetPermissionRequestId) : IIntegrationEvent;