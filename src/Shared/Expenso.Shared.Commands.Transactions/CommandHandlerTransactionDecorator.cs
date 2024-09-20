using Expenso.Shared.Database;

namespace Expenso.Shared.Commands.Transactions;

internal sealed class CommandHandlerTransactionDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public CommandHandlerTransactionDecorator(IUnitOfWork unitOfWork, ICommandHandler<TCommand> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(paramName: nameof(unitOfWork));
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
            await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken: cancellationToken);

            throw;
        }
    }
}

internal sealed class CommandHandlerTransactionDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;

    public CommandHandlerTransactionDecorator(IUnitOfWork unitOfWork, ICommandHandler<TCommand, TResult> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(paramName: nameof(unitOfWork));
    }

    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            TResult? commandResult =
                await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken: cancellationToken);

            return commandResult;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken: cancellationToken);

            throw;
        }
    }
}