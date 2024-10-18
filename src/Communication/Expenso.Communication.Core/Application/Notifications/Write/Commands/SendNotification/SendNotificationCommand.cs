using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

public sealed record SendNotificationCommand(IMessageContext MessageContext, SendNotificationRequest? Payload)
    : ICommand;