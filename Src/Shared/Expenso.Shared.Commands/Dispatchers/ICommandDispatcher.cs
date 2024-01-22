namespace Expenso.Shared.Commands.Dispatchers;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand;

    Task<TResult?> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand where TResult : class;
}