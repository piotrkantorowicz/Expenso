namespace Expenso.Shared.Commands;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand where TResult : class
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}