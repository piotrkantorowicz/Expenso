namespace Expenso.Shared.Commands.Validation;

public interface ICommandValidator<TCommand> where TCommand : class, ICommand
{
    IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> GetValidationMetadata();

    IDictionary<string, string> Validate(TCommand command);
}