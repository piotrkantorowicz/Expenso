using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJobEntry.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJobEntry;

public sealed record CancelJobEntryCommand(IMessageContext MessageContext, CancelJobEntryRequest? Payload) : ICommand;