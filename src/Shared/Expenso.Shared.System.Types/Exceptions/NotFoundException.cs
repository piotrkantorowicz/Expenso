using System.Text;

using Expenso.Shared.System.Types.Constants;
using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.Shared.System.Types.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message: message)
    {
    }

    public NotFoundException(string resourceName, IdentifierType? identifierType = null, object? identifier = null) :
        base(message: BuildMessage(resourceName: resourceName, identifierType: identifierType, identifier: identifier,
            restOfMessage: "hasn't been found"))
    {
        ResourceName = resourceName;
        IdentifierType = identifierType;
        Identifier = identifier;
    }

    public string? ResourceName { get; }

    public IdentifierType? IdentifierType { get; }

    public object? Identifier { get; }

    private static string BuildMessage(string resourceName, IdentifierType? identifierType, object? identifier,
        string restOfMessage)
    {
        StringBuilder stringBuilder =
            new StringBuilder().Append(value: resourceName).Append(value: Characters.Separator);

        if (identifierType is not null)
        {
            stringBuilder.Append(value: "with ").Append(value: identifierType).Append(value: Characters.Separator);
        }

        if (identifier is not null)
        {
            stringBuilder.Append(value: identifier).Append(value: Characters.Separator);
        }

        stringBuilder.Append(value: restOfMessage).Append(value: Characters.Dot);

        return stringBuilder.ToString();
    }
}