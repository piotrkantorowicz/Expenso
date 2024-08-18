using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

public sealed record RegisterJobEntryCommand(
    IMessageContext MessageContext,
    RegisterJobEntryRequest? RegisterJobEntryRequest) : ICommand;