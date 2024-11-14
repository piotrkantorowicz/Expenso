using System.Text;

using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.Shared.System.Types.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message: message)
    {
    }

    public NotFoundException(string resourceName, IdentifierType? identifierType = null, object? identifier = null) :
        base(message: BuildMessage(resourceName: resourceName, identifierType: identifierType, identifier: identifier))
    {
    }

    private static string BuildMessage(string resourceName, IdentifierType? identifierType, object? identifier)
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

        stringBuilder.Append(value: "hasn't been found.");

        return stringBuilder.ToString();
    }
}