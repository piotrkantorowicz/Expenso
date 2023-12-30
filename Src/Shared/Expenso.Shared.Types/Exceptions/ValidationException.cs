using System.Text;

namespace Expenso.Shared.Types.Exceptions;

public sealed class ValidationException : Exception
{
    private const string DefaultMessage = "One or more validation fa" + "ilures have occurred.";

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
        Details = details;
        CreateErrorDictionary(errorDictionary);
    }

    public string? Details { get; set; }

    public IDictionary<string, string> ErrorDictionary { get; set; } = new Dictionary<string, string>();

    private void CreateErrorDictionary(IDictionary<string, string> errorDictionary)
    {
        StringBuilder stringBuilder = new();

        foreach ((string key, string value) in errorDictionary)
        {
            stringBuilder.AppendLine($"{key}: {value}.");
        }

        Details = stringBuilder.ToString();
        ErrorDictionary = errorDictionary;
    }
}