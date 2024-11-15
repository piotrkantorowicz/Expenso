namespace Expenso.Shared.System.Types.Exceptions.Models;

public sealed class IdentifierType
{
    private IdentifierType(string value)
    {
        if (string.IsNullOrWhiteSpace(value: value))
        {
            throw new ArgumentException(message: "Identifier type value cannot be empty or whitespace.",
                paramName: nameof(value));
        }

        Value = value;
    }

    public static IdentifierType PrimaryId()
    {
        return new IdentifierType(value: "ID");
    }

    public static IdentifierType Email()
    {
        return new IdentifierType(value: "email");
    }

    public static IdentifierType Query()
    {
        return new IdentifierType(value: "query");
    }

    public static IdentifierType Custom(string value)
    {
        return new IdentifierType(value: value);
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}