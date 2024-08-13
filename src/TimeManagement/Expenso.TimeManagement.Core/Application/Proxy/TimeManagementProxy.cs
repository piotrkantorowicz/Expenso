using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy;
using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Proxy;

internal sealed class TimeManagementProxy(
    ICommandDispatcher commandDispatcher,
    IMessageContextFactory messageContextFactory) : ITimeManagementProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory = messageContextFactory ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(messageContextFactory));

    public async Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<RegisterJobCommand, RegisterJobEntryResponse>(
            command: new RegisterJobCommand(MessageContext: _messageContextFactory.Current(),
                RegisterJobEntryRequest: jobEntryRequest), cancellationToken: cancellationToken);
    }
}