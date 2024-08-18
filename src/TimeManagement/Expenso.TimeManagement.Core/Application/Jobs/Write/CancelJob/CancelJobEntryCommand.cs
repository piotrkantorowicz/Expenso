using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;

public sealed record CancelJobEntryCommand(IMessageContext MessageContext, CancelJobEntryRequest? CancelJobEntryRequest)
    : ICommand;