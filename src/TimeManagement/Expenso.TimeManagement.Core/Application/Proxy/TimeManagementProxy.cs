using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJobEntry;
using Expenso.TimeManagement.Shared;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

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

    public async Task<RegisterJobEntryResponse?> RegisterJobEntry(RegisterJobEntryRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<RegisterJobEntryCommand, RegisterJobEntryResponse>(
            command: new RegisterJobEntryCommand(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.TimeManagementModule), Payload: request),
            cancellationToken: cancellationToken);
    }
}