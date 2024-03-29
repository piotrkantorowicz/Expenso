using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Dispatchers;

internal sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : class, ICommand
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ICommandHandler<TCommand>? handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found.");
        }

        await handler.HandleAsync(command, cancellationToken);
    }

    public async Task<TResult?> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : class, ICommand where TResult : class
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        ICommandHandler<TCommand, TResult>? handler =
            scope.ServiceProvider.GetService<ICommandHandler<TCommand, TResult>>();

        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found.");
        }

        return await handler.HandleAsync(command, cancellationToken);
    }
}