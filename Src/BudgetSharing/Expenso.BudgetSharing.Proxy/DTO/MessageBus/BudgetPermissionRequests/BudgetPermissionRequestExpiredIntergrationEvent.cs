﻿using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;

public sealed record BudgetPermissionRequestExpiredIntergrationEvent(
    IMessageContext MessageContext,
    Guid BudgetPermissionRequestId) : IIntegrationEvent;