using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Proxy;

internal sealed class TimeManagementProxy(
    ICommandDispatcher commandDispatcher,
    IMessageContextFactory messageContextFactory) : ITimeManagementProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    public async Task RegisterJobEntry(AddJobEntryRequest jobEntryRequest,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(new RegisterJobCommand(_messageContextFactory.Current(), jobEntryRequest),
            cancellationToken);
    }
}