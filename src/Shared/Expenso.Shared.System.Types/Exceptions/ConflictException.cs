using System.Text;

using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message: message)
    {
    }

    private ConflictException(string resourceName, IdentifierType? identifierType, object? identifier,
        string restOfMessage) : base(message: BuildMessage(resourceName: resourceName, identifierType: identifierType,
        identifier: identifier, restOfMessage: restOfMessage))
    {
    }

    public static ConflictException AlreadyExists(string resourceName, IdentifierType? identifierType = null,
        object? identifier = null)
    {
        return new ConflictException(resourceName: resourceName, identifierType: identifierType, identifier: identifier,
            restOfMessage: "already exists");
    }

    private static string BuildMessage(string resourceName, IdentifierType? identifierType, object? identifier,
        string restOfMessage)
    {
        StringBuilder stringBuilder = new StringBuilder().Append(value: resourceName).Append(value: ' ');

        if (identifierType is not null)
        {
            stringBuilder.Append(value: "with ").Append(value: identifierType).Append(value: ' ');
        }

        if (identifier is not null)
        {
            stringBuilder.Append(value: identifier).Append(value: ' ');
        }

        stringBuilder.Append(value: restOfMessage).Append(value: '.');

        return stringBuilder.ToString();
    }
}