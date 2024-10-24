using System.Text;

using Humanizer;

namespace Expenso.Shared.System.Types.Exceptions;

public class ValidationException : Exception
{
    private const string DefaultMessage = "One or more validation failures have occurred.";

    public ValidationException(string details) : base(message: DefaultMessage)
    {
        Details = details;
    }

    public ValidationException(IDictionary<string, string> errorDictionary) : base(message: DefaultMessage)
    {
        CreateErrorDictionary(errorDictionary: errorDictionary);
    }

    public string? Details { get; private set; }

    public IDictionary<string, string> ErrorDictionary { get; set; } = new Dictionary<string, string>();

    private void CreateErrorDictionary(IDictionary<string, string> errorDictionary, string? details = null)
    {
        StringBuilder stringBuilder = new();

        if (details is not null)
        {
            stringBuilder.AppendLine(value: details);
        }

        foreach ((string key, string value) in errorDictionary)
        {
            stringBuilder.AppendLine(handler: $"{key.Pascalize()}: {value}");
        }

        Details = stringBuilder.ToString();
        ErrorDictionary = errorDictionary;
    }
}