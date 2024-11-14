namespace Expenso.Shared.System.Types.Exceptions.Models;

public sealed class IdentifierType
{
    private IdentifierType(string? value)
    {
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

    public static IdentifierType Custom(string value)
    {
        return new IdentifierType(value: value);
    }

    public string? Value { get; set; }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}