using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry;

public sealed record CancelJobEntryCommand(IMessageContext MessageContext, CancelJobEntryRequest? Payload) : ICommand;