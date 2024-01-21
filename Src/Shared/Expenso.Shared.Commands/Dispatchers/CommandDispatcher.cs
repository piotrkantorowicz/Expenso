using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Dispatchers;

internal sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ICommandHandler<TCommand> handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }

    public async Task<TResult?> SendAsync<TCommand, TResult>(TCommand command,
        CancellationToken cancellationToken = default) where TCommand : class, ICommand where TResult : class
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        ICommandHandler<TCommand, TResult> handler =
            scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

        return await handler.HandleAsync(command, cancellationToken);
    }
}