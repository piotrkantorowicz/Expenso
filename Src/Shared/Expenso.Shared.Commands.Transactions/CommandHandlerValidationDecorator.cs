using Expenso.Shared.Database;

namespace Expenso.Shared.Commands.Transactions;

internal class CommandHandlerValidationDecorator<TCommand>(IUnitOfWork unitOfWork, ICommandHandler<TCommand> decorated)
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await decorated.HandleAsync(command, cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
    }
}

internal class CommandHandlerValidationDecorator<TCommand, TResult>(
    IUnitOfWork unitOfWork,
    ICommandHandler<TCommand, TResult> decorated)
    : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        TResult? commandResult = await decorated.HandleAsync(command, cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return commandResult;
    }
}