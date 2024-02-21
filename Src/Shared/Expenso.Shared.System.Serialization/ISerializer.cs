namespace Expenso.Shared.System.Serialization;

public interface ISerializer
{
    T? Deserialize<T>(string value, object? settings = null);

    object? Deserialize(string value, Type type, object? settings = null);

    string Serialize<T>(T value, object? settings = null);
}