using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Shared;
using Expenso.TimeManagement.Shared.DTO.Request;
using Expenso.TimeManagement.Shared.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Proxy;

internal sealed class TimeManagementProxy : ITimeManagementProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageContextFactory _messageContextFactory;

    public TimeManagementProxy(ICommandDispatcher commandDispatcher, IMessageContextFactory messageContextFactory)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));
    }

    public async Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest jobEntryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<RegisterJobEntryCommand, RegisterJobEntryResponse>(
            command: new RegisterJobEntryCommand(MessageContext: _messageContextFactory.Current(),
                Payload: jobEntryRequest), cancellationToken: cancellationToken);
    }
}