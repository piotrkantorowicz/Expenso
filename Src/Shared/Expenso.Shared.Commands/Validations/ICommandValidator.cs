namespace Expenso.Shared.Commands.Validations;

public interface ICommandValidator<in TCommand> where TCommand : class, ICommand
{
    IDictionary<string, string> Validate(TCommand command);
}