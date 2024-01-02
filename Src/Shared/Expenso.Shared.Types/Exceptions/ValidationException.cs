using System.Text;

using Humanizer;

namespace Expenso.Shared.Types.Exceptions;

public sealed class ValidationException : Exception
{
    private const string DefaultMessage = "One or more validation failures have occurred.";

    public ValidationException(string details) : base(DefaultMessage)
    {
        Details = details;
    }

    public ValidationException(IDictionary<string, string> errorDictionary) : base(DefaultMessage)
    {
        CreateErrorDictionary(errorDictionary);
    }

    public ValidationException(string details, IDictionary<string, string> errorDictionary) : base(DefaultMessage)
    {
        CreateErrorDictionary(errorDictionary, details);
    }

    public string? Details { get; private set; }

    public IDictionary<string, string> ErrorDictionary { get; set; } = new Dictionary<string, string>();

    private void CreateErrorDictionary(IDictionary<string, string> errorDictionary, string? details = null)
    {
        StringBuilder stringBuilder = new();

        if (details is not null)
        {
            stringBuilder.AppendLine(details);
        }

        foreach ((string key, string value) in errorDictionary)
        {
            stringBuilder.AppendLine($"{key.Pascalize()}: {value}");
        }

        Details = stringBuilder.ToString();
        ErrorDictionary = errorDictionary;
    }
}