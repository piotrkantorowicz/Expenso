namespace Expenso.Shared.Commands.Validation;

public sealed record CommandValidationRule<TCommand> where TCommand : class, ICommand
{
    private readonly Func<TCommand?, string> _createMessageFunc;
    private readonly Func<TCommand?, bool> _isValidationFailedFunc;

    public CommandValidationRule(Func<TCommand?, bool> validationFailedFunc, Func<TCommand?, string> createMessageFunc,
        string validationType, object? value)
    {
        _isValidationFailedFunc = validationFailedFunc;
        _createMessageFunc = createMessageFunc;
        ValidationType = validationType;
        Value = value;
    }

    public string ValidationType { get; }

    public object? Value { get; }

    public (string Property, string ErrorMessage)? Validate(TCommand? command, string property)
    {
        if (_isValidationFailedFunc(arg: command))
        {
            return (property, _createMessageFunc(arg: command));
        }

        return null;
    }
}