using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJobEntry;

public sealed record RegisterJobEntryCommand(IMessageContext MessageContext, RegisterJobEntryRequest? Payload)
    : ICommand;