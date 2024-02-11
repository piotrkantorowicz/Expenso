namespace Expenso.Shared.Commands.Validation;

public interface ICommandValidator<in TCommand> where TCommand : class, ICommand
{
    IDictionary<string, string> Validate(TCommand command);
}