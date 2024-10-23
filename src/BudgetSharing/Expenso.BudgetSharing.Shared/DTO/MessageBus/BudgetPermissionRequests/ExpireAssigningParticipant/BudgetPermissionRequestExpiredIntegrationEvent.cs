using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;

public sealed record BudgetPermissionRequestExpiredIntegrationEvent(
    IMessageContext MessageContext,
    BudgetPermissionRequestExpiredPayload Payload) : IIntegrationEvent;